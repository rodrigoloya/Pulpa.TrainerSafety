# Subscription Management & Billing System

## Overview
The Phishing Trainer application implements a freemium subscription model with Stripe integration for payment processing. The system supports monthly billing, subscription management, and automatic feature provisioning based on subscription level.

## Subscription Model

### Subscription Tiers

#### Free Tier ($0/month)
- **Campaigns**: 1 active campaign at a time
- **Users**: Personal use only (single user)
- **Templates**: Basic templates only
- **Analytics**: Basic results (7-day history)
- **Family Groups**: Not available
- **Campaign Scheduling**: Immediate launch only
- **Support**: Community support

#### Pro Tier ($10/month)
- **Campaigns**: Unlimited active campaigns
- **Users**: Up to 5 family members
- **Templates**: Basic + custom templates
- **Analytics**: Advanced analytics (90-day history)
- **Family Groups**: Create and manage family groups
- **Campaign Scheduling**: Advanced scheduling options
- **Support**: Priority email support

### Subscription Entity Model
```csharp
public class Subscription
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    public SubscriptionType Type { get; set; }
    public SubscriptionStatus Status { get; set; }
    public string? StripeCustomerId { get; set; }
    public string? StripeSubscriptionId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool AutoRenew { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? LastPaymentError { get; set; }
    public virtual ICollection<SubscriptionTransaction> Transactions { get; set; }
}

public enum SubscriptionType
{
    Free = 0,
    Pro = 1
}

public enum SubscriptionStatus
{
    Active = 0,
    PastDue = 1,
    Cancelled = 2,
    Unpaid = 3,
    Trialing = 4
}

public class SubscriptionTransaction
{
    public string Id { get; set; }
    public string SubscriptionId { get; set; }
    public virtual Subscription Subscription { get; set; }
    public string StripeTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
    public string? FailureReason { get; set; }
}

public enum TransactionType
{
    Payment = 0,
    Refund = 1,
    Chargeback = 2
}

public enum TransactionStatus
{
    Succeeded = 0,
    Pending = 1,
    Failed = 2
}
```

## Stripe Integration

### Stripe Configuration
```csharp
public class StripeConfiguration
{
    public string PublishableKey { get; set; }
    public string SecretKey { get; set; }
    public string WebhookSecret { get; set; }
    public string ProPriceId { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}

public class StripeService
{
    private readonly IConfiguration _config;
    private readonly ILogger<StripeService> _logger;

    public StripeService(IConfiguration config, ILogger<StripeService> logger)
    {
        _config = config;
        _logger = logger;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<Customer> CreateCustomerAsync(string email, string name)
    {
        var options = new CustomerCreateOptions
        {
            Email = email,
            Name = name,
            Metadata = new Dictionary<string, string>
            {
                ["app"] = "phishing-trainer"
            }
        };

        var service = new CustomerService();
        return await service.CreateAsync(options);
    }

    public async Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId)
    {
        var options = new SubscriptionCreateOptions
        {
            Customer = customerId,
            Items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions
                {
                    Price = priceId,
                },
            },
            PaymentBehavior = "default_incomplete",
            Expand = ["latest_invoice.payment_intent"],
        };

        var service = new SubscriptionService();
        return await service.CreateAsync(options);
    }

    public async Task<Subscription> CancelSubscriptionAsync(string subscriptionId)
    {
        var service = new SubscriptionService();
        return await service.CancelAsync(subscriptionId, new SubscriptionCancelOptions
        {
            CancelAtPeriodEnd = true
        });
    }

    public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency, string customerId)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Convert to cents
            Currency = currency,
            Customer = customerId,
            Metadata = new Dictionary<string, string>
            {
                ["app"] = "phishing-trainer"
            },
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
        };

        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }
}
```

### Subscription Management Service
```csharp
public class SubscriptionService
{
    private readonly ApplicationDbContext _context;
    private readonly StripeService _stripeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<SubscriptionService> _logger;

    public async Task<SubscriptionResult> CreateSubscriptionAsync(string userId, string paymentMethodId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return SubscriptionResult.Failed("User not found");

        try
        {
            // Create or get Stripe customer
            var customerId = user.StripeCustomerId;
            if (string.IsNullOrEmpty(customerId))
            {
                var customer = await _stripeService.CreateCustomerAsync(
                    user.Email!, 
                    $"{user.FirstName} {user.LastName}");
                customerId = customer.Id;
                user.StripeCustomerId = customerId;
                await _userManager.UpdateAsync(user);
            }

            // Attach payment method to customer
            await _stripeService.AttachPaymentMethodAsync(customerId, paymentMethodId);

            // Create Stripe subscription
            var stripeSubscription = await _stripeService.CreateSubscriptionAsync(
                customerId, 
                _config["Stripe:ProPriceId"]);

            // Create local subscription record
            var subscription = new Subscription
            {
                UserId = userId,
                Type = SubscriptionType.Pro,
                Status = SubscriptionStatus.Trialing,
                StripeCustomerId = customerId,
                StripeSubscriptionId = stripeSubscription.Id,
                StartedAt = DateTime.UtcNow,
                CurrentPeriodStart = stripeSubscription.CurrentPeriodStart,
                CurrentPeriodEnd = stripeSubscription.CurrentPeriodEnd,
                AutoRenew = true,
                Amount = 10.00m,
                Currency = "usd"
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            // Update user subscription type
            user.SubscriptionType = SubscriptionType.Pro;
            await _userManager.UpdateAsync(user);

            return SubscriptionResult.Success(subscription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create subscription for user {UserId}", userId);
            return SubscriptionResult.Failed("Failed to create subscription");
        }
    }

    public async Task<SubscriptionResult> CancelSubscriptionAsync(string userId)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active);

        if (subscription == null)
            return SubscriptionResult.Failed("No active subscription found");

        try
        {
            // Cancel in Stripe
            await _stripeService.CancelSubscriptionAsync(subscription.StripeSubscriptionId);

            // Update local record
            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.CancelledAt = DateTime.UtcNow;
            subscription.AutoRenew = false;

            await _context.SaveChangesAsync();

            return SubscriptionResult.Success(subscription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel subscription for user {UserId}", userId);
            return SubscriptionResult.Failed("Failed to cancel subscription");
        }
    }

    public async Task<SubscriptionResult> ReactivateSubscriptionAsync(string userId)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Cancelled);

        if (subscription == null)
            return SubscriptionResult.Failed("No cancelled subscription found");

        try
        {
            // Reactivate in Stripe
            await _stripeService.ReactivateSubscriptionAsync(subscription.StripeSubscriptionId);

            // Update local record
            subscription.Status = SubscriptionStatus.Active;
            subscription.CancelledAt = null;
            subscription.AutoRenew = true;

            await _context.SaveChangesAsync();

            return SubscriptionResult.Success(subscription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reactivate subscription for user {UserId}", userId);
            return SubscriptionResult.Failed("Failed to reactivate subscription");
        }
    }
}
```

## Webhook Handling

### Stripe Webhook Controller
```csharp
[ApiController]
[Route("api/webhooks/stripe")]
public class StripeWebhookController : ControllerBase
{
    private readonly IWebhookProcessor _webhookProcessor;
    private readonly ILogger<StripeWebhookController> _logger;

    [HttpPost]
    public async Task<IActionResult> ProcessWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signatureHeader = Request.Headers["Stripe-Signature"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signatureHeader,
                _config["Stripe:WebhookSecret"]
            );

            await _webhookProcessor.ProcessEventAsync(stripeEvent);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process Stripe webhook");
            return BadRequest();
        }
    }
}

public class WebhookProcessor : IWebhookProcessor
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WebhookProcessor> _logger;

    public async Task ProcessEventAsync(Event stripeEvent)
    {
        switch (stripeEvent.Type)
        {
            case "invoice.payment_succeeded":
                await HandleInvoicePaymentSucceeded(stripeEvent);
                break;
            case "invoice.payment_failed":
                await HandleInvoicePaymentFailed(stripeEvent);
                break;
            case "customer.subscription.deleted":
                await HandleSubscriptionDeleted(stripeEvent);
                break;
            case "customer.subscription.updated":
                await HandleSubscriptionUpdated(stripeEvent);
                break;
        }
    }

    private async Task HandleInvoicePaymentSucceeded(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == invoice.SubscriptionId);

        if (subscription != null)
        {
            subscription.Status = SubscriptionStatus.Active;
            subscription.CurrentPeriodStart = invoice.PeriodStart;
            subscription.CurrentPeriodEnd = invoice.PeriodEnd;
            subscription.LastPaymentError = null;

            // Record transaction
            _context.SubscriptionTransactions.Add(new SubscriptionTransaction
            {
                SubscriptionId = subscription.Id,
                StripeTransactionId = invoice.PaymentIntentId,
                Amount = invoice.AmountPaid / 100m,
                Currency = invoice.Currency,
                Type = TransactionType.Payment,
                Status = TransactionStatus.Succeeded,
                CreatedAt = DateTime.UtcNow,
                Description = "Monthly subscription payment"
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task HandleInvoicePaymentFailed(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == invoice.SubscriptionId);

        if (subscription != null)
        {
            subscription.Status = SubscriptionStatus.PastDue;
            subscription.LastPaymentError = invoice.LastFinalizationError?.Message;

            await _context.SaveChangesAsync();
        }
    }

    private async Task HandleSubscriptionDeleted(Event stripeEvent)
    {
        var stripeSubscription = stripeEvent.Data.Object as Subscription;
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSubscription.Id);

        if (subscription != null)
        {
            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.EndedAt = DateTime.UtcNow;

            // Update user to free tier
            var user = await _userManager.FindByIdAsync(subscription.UserId);
            if (user != null)
            {
                user.SubscriptionType = SubscriptionType.Free;
                await _userManager.UpdateAsync(user);
            }

            await _context.SaveChangesAsync();
        }
    }
}
```

## API Controllers

### Subscription Management API
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;

    [HttpGet("current")]
    public async Task<ActionResult<SubscriptionDto>> GetCurrentSubscription()
    {
        var userId = User.FindFirstValue(CustomClaims.UserId);
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);
        
        if (subscription == null)
        {
            return Ok(new SubscriptionDto
            {
                Type = SubscriptionType.Free,
                Status = SubscriptionStatus.Active,
                Amount = 0,
                Currency = "usd"
            });
        }

        return Ok(subscription.ToDto());
    }

    [HttpPost("upgrade")]
    [RequireSubscription(SubscriptionType.Free)]
    public async Task<ActionResult<SubscriptionDto>> UpgradeSubscription(UpgradeSubscriptionRequest request)
    {
        var userId = User.FindFirstValue(CustomClaims.UserId);
        
        var result = await _subscriptionService.CreateSubscriptionAsync(userId, request.PaymentMethodId);
        
        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Subscription.ToDto());
    }

    [HttpPost("cancel")]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult> CancelSubscription()
    {
        var userId = User.FindFirstValue(CustomClaims.UserId);
        
        var result = await _subscriptionService.CancelSubscriptionAsync(userId);
        
        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok();
    }

    [HttpPost("reactivate")]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult<SubscriptionDto>> ReactivateSubscription()
    {
        var userId = User.FindFirstValue(CustomClaims.UserId);
        
        var result = await _subscriptionService.ReactivateSubscriptionAsync(userId);
        
        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Subscription.ToDto());
    }

    [HttpGet("billing-history")]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetBillingHistory()
    {
        var userId = User.FindFirstValue(CustomClaims.UserId);
        var transactions = await _subscriptionService.GetBillingHistoryAsync(userId);
        
        return Ok(transactions.Select(t => t.ToDto()));
    }
}
```

## Frontend Integration

### Subscription Management Component
```razor
@page "/subscription"
@inject ISubscriptionService SubscriptionService
@inject IToastService ToastService

<h3>Subscription Management</h3>

@if (currentSubscription == null)
{
    <p>Loading...</p>
}
else
{
    <div class="subscription-card">
        <h4>Current Plan: @currentSubscription.Type</h4>
        <p>Status: @currentSubscription.Status</p>
        
        @if (currentSubscription.Type == SubscriptionType.Free)
        {
            <div class="upgrade-section">
                <h5>Upgrade to Pro</h5>
                <ul>
                    <li>Unlimited campaigns</li>
                    <li>Up to 5 family members</li>
                    <li>Custom templates</li>
                    <li>Advanced analytics</li>
                </ul>
                <button class="btn btn-primary" @onclick="StartUpgradeProcess">
                    Upgrade for $10/month
                </button>
            </div>
        }
        else
        {
            <div class="pro-section">
                <p>Amount: $@currentSubscription.Amount/month</p>
                <p>Next billing: @currentSubscription.CurrentPeriodEnd?.ToString("MMMM dd, yyyy")</p>
                
                @if (currentSubscription.Status == SubscriptionStatus.Cancelled)
                {
                    <button class="btn btn-success" @onclick="ReactivateSubscription">
                        Reactivate Subscription
                    </button>
                }
                else
                {
                    <button class="btn btn-warning" @onclick="CancelSubscription">
                        Cancel Subscription
                    </button>
                }
            </div>
        }
    </div>
}

@if (showPaymentForm)
{
    <PaymentForm OnPaymentCompleted="HandlePaymentCompleted" />
}

@code {
    private SubscriptionDto currentSubscription;
    private bool showPaymentForm;

    protected override async Task OnInitializedAsync()
    {
        await LoadCurrentSubscription();
    }

    private async Task LoadCurrentSubscription()
    {
        currentSubscription = await SubscriptionService.GetCurrentSubscriptionAsync();
    }

    private void StartUpgradeProcess()
    {
        showPaymentForm = true;
    }

    private async Task HandlePaymentCompleted(PaymentResult result)
    {
        if (result.Success)
        {
            showPaymentForm = false;
            await LoadCurrentSubscription();
            ToastService.ShowSuccess("Subscription upgraded successfully!");
        }
        else
        {
            ToastService.ShowError("Payment failed: " + result.ErrorMessage);
        }
    }

    private async Task CancelSubscription()
    {
        var result = await SubscriptionService.CancelSubscriptionAsync();
        if (result.Success)
        {
            await LoadCurrentSubscription();
            ToastService.ShowSuccess("Subscription cancelled. You'll continue to have access until the end of your billing period.");
        }
        else
        {
            ToastService.ShowError("Failed to cancel subscription: " + result.ErrorMessage);
        }
    }

    private async Task ReactivateSubscription()
    {
        var result = await SubscriptionService.ReactivateSubscriptionAsync();
        if (result.Success)
        {
            await LoadCurrentSubscription();
            ToastService.ShowSuccess("Subscription reactivated successfully!");
        }
        else
        {
            ToastService.ShowError("Failed to reactivate subscription: " + result.ErrorMessage);
        }
    }
}
```

## Billing Analytics

### Revenue Tracking
```csharp
public class BillingAnalyticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BillingAnalyticsService> _logger;

    public async Task<BillingMetrics> GetBillingMetricsAsync(DateTime startDate, DateTime endDate)
    {
        var transactions = await _context.SubscriptionTransactions
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToListAsync();

        var activeSubscriptions = await _context.Subscriptions
            .CountAsync(s => s.Status == SubscriptionStatus.Active);

        var newSubscriptions = await _context.Subscriptions
            .CountAsync(s => s.StartedAt >= startDate && s.StartedAt <= endDate);

        var cancelledSubscriptions = await _context.Subscriptions
            .CountAsync(s => s.CancelledAt >= startDate && s.CancelledAt <= endDate);

        return new BillingMetrics
        {
            TotalRevenue = transactions.Where(t => t.Type == TransactionType.Payment && t.Status == TransactionStatus.Succeeded).Sum(t => t.Amount),
            ActiveSubscriptions = activeSubscriptions,
            NewSubscriptions = newSubscriptions,
            CancelledSubscriptions = cancelledSubscriptions,
            ChurnRate = CalculateChurnRate(newSubscriptions, cancelledSubscriptions),
            MonthlyRecurringRevenue = await CalculateMRRAsync()
        };
    }

    private decimal CalculateMRRAsync()
    {
        return _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active)
            .Sum(s => s.Amount);
    }
}
```

## Security & Compliance

### PCI Compliance
- All payment processing handled through Stripe (PCI compliant)
- No credit card data stored in application database
- Use of Stripe Elements for secure payment form
- HTTPS enforcement for all payment-related endpoints

### Data Protection
- Secure storage of subscription data
- Audit logging for all subscription changes
- GDPR compliance for customer data
- Data retention policies for billing records

This subscription and billing system provides:

1. **Complete subscription lifecycle management** with Stripe integration
2. **Secure payment processing** without storing sensitive card data
3. **Automated provisioning** of features based on subscription level
4. **Comprehensive webhook handling** for real-time subscription updates
5. **Billing analytics** and revenue tracking
6. **User-friendly frontend components** for subscription management
7. **Compliance with security standards** and data protection regulations

The system ensures smooth upgrade/downgrade experiences while maintaining security and reliability throughout the billing process.