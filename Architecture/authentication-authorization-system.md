# Authentication & Authorization System Design

## Overview
The Phishing Trainer application implements a robust authentication and authorization system using ASP.NET Core Identity with JWT tokens for secure API access and role-based access control (RBAC).

## Authentication Architecture

### User Identity Model
```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public DateTime? PhoneVerifiedAt { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public string? FamilyGroupId { get; set; }
    
    // Navigation properties
    public virtual FamilyGroup? FamilyGroup { get; set; }
    public virtual ICollection<UserLogin> Logins { get; set; }
    public virtual ICollection<UserToken> Tokens { get; set; }
    public virtual ICollection<UserClaim> Claims { get; set; }
}

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### JWT Token Configuration
```csharp
public class JwtConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int AccessTokenExpiration { get; set; } // minutes
    public int RefreshTokenExpiration { get; set; } // days
}

// JWT Claims Structure
public class CustomClaims
{
    public const string UserId = "uid";
    public const string SubscriptionType = "sub_type";
    public const string FamilyGroupId = "family_id";
    public const string Permissions = "permissions";
    public const string SecurityStamp = "security_stamp";
}
```

### Authentication Flow Implementation

#### Login Service
```csharp
public class AuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationService> _logger;

    public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return AuthenticationResult.Failed("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return AuthenticationResult.Failed("Invalid credentials");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate tokens
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

        return AuthenticationResult.Success(accessToken, refreshToken, user);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirstValue(CustomClaims.UserId);
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.RefreshToken != request.RefreshToken || 
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return AuthenticationResult.Failed("Invalid refresh token");
        }

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

        return AuthenticationResult.Success(newAccessToken, newRefreshToken, user);
    }
}
```

#### Token Service
```csharp
public class TokenService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly UserManager<ApplicationUser> _userManager;

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var claims = await GetClaimsAsync(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(CustomClaims.UserId, user.Id),
            new Claim(CustomClaims.SubscriptionType, user.SubscriptionType.ToString()),
            new Claim(CustomClaims.SecurityStamp, user.SecurityStamp ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(user.FamilyGroupId))
        {
            claims.Add(new Claim(CustomClaims.FamilyGroupId, user.FamilyGroupId));
        }

        // Add role claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Add permission claims based on subscription
        var permissions = GetPermissionsForSubscription(user.SubscriptionType);
        claims.AddRange(permissions.Select(p => new Claim(CustomClaims.Permissions, p)));

        return claims;
    }

    private IEnumerable<string> GetPermissionsForSubscription(SubscriptionType subscriptionType)
    {
        return subscriptionType switch
        {
            SubscriptionType.Free => new[] { "create_campaign", "view_results" },
            SubscriptionType.Pro => new[] { 
                "create_campaign", "view_results", "create_custom_template", 
                "manage_family", "advanced_analytics", "schedule_campaigns"
            },
            _ => Array.Empty<string>()
        };
    }
}
```

## Authorization System

### Permission-Based Access Control
```csharp
public class Permissions
{
    // Campaign permissions
    public const string CreateCampaign = "create_campaign";
    public const string ViewCampaigns = "view_campaigns";
    public const string EditCampaign = "edit_campaign";
    public const string DeleteCampaign = "delete_campaign";
    public const string ScheduleCampaigns = "schedule_campaigns";
    
    // Template permissions
    public const string ViewTemplates = "view_templates";
    public const string CreateCustomTemplate = "create_custom_template";
    public const string EditCustomTemplate = "edit_custom_template";
    
    // Analytics permissions
    public const string ViewResults = "view_results";
    public const string AdvancedAnalytics = "advanced_analytics";
    public const string ExportReports = "export_reports";
    
    // Family management
    public const string ManageFamily = "manage_family";
    public const string InviteFamilyMembers = "invite_family_members";
    
    // System permissions
    public const string ManageSubscription = "manage_subscription";
    public const string ViewBilling = "view_billing";
}

public class SubscriptionPermissions
{
    public static readonly Dictionary<SubscriptionType, string[]> PermissionsMap = new()
    {
        {
            SubscriptionType.Free, 
            new[]
            {
                Permissions.ViewCampaigns,
                Permissions.CreateCampaign,
                Permissions.ViewResults,
                Permissions.ViewTemplates
            }
        },
        {
            SubscriptionType.Pro, 
            new[]
            {
                Permissions.ViewCampaigns,
                Permissions.CreateCampaign,
                Permissions.EditCampaign,
                Permissions.DeleteCampaign,
                Permissions.ScheduleCampaigns,
                Permissions.ViewTemplates,
                Permissions.CreateCustomTemplate,
                Permissions.EditCustomTemplate,
                Permissions.ViewResults,
                Permissions.AdvancedAnalytics,
                Permissions.ExportReports,
                Permissions.ManageFamily,
                Permissions.InviteFamilyMembers,
                Permissions.ManageSubscription,
                Permissions.ViewBilling
            }
        }
    };
}
```

### Authorization Attributes
```csharp
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission) : base($"Permission:{permission}")
    {
    }
}

public class RequireSubscriptionAttribute : AuthorizeAttribute
{
    public RequireSubscriptionAttribute(params SubscriptionType[] subscriptionTypes) 
        : base($"Subscription:{string.Join(",", subscriptionTypes)}")
    {
    }
}
```

### Authorization Handlers
```csharp
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var permissionClaims = context.User.FindAll(CustomClaims.Permissions);
        
        if (permissionClaims.Any(claim => claim.Value == requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class SubscriptionAuthorizationHandler : AuthorizationHandler<SubscriptionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SubscriptionRequirement requirement)
    {
        var subscriptionClaim = context.User.FindFirst(CustomClaims.SubscriptionType);
        
        if (subscriptionClaim != null && 
            Enum.TryParse<SubscriptionType>(subscriptionClaim.Value, out var userSubscription))
        {
            if (requirement.AllowedSubscriptions.Contains(userSubscription))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

public class FamilyMemberAuthorizationHandler : AuthorizationHandler<FamilyMemberRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        FamilyMemberRequirement requirement)
    {
        var userId = context.User.FindFirstValue(CustomClaims.UserId);
        var familyGroupId = context.User.FindFirstValue(CustomClaims.FamilyGroupId);
        
        // User can access their own resources
        if (userId == requirement.TargetUserId)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Users in the same family group can access each other's resources
        if (!string.IsNullOrEmpty(familyGroupId) && 
            familyGroupId == requirement.TargetFamilyGroupId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

### API Controller Security
```csharp
[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.ViewCampaigns)]
    public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaigns()
    {
        // Implementation
    }

    [HttpPost]
    [RequirePermission(Permissions.CreateCampaign)]
    public async Task<ActionResult<CampaignDto>> CreateCampaign(CreateCampaignRequest request)
    {
        // Implementation
    }

    [HttpPost("{id}/schedule")]
    [RequirePermission(Permissions.ScheduleCampaigns)]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult> ScheduleCampaign(string id, ScheduleCampaignRequest request)
    {
        // Implementation
    }

    [HttpGet("{id}/results")]
    [RequirePermission(Permissions.ViewResults)]
    public async Task<ActionResult<CampaignResultsDto>> GetCampaignResults(string id)
    {
        // Implementation
    }
}

[ApiController]
[Route("api/[controller]")]
public class FamilyController : ControllerBase
{
    [HttpPost("groups")]
    [RequirePermission(Permissions.ManageFamily)]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult<FamilyGroupDto>> CreateFamilyGroup(CreateFamilyGroupRequest request)
    {
        // Implementation
    }

    [HttpPost("groups/{groupId}/members")]
    [RequirePermission(Permissions.InviteFamilyMembers)]
    [RequireSubscription(SubscriptionType.Pro)]
    public async Task<ActionResult> InviteFamilyMember(string groupId, InviteFamilyMemberRequest request)
    {
        // Implementation
    }
}
```

## Security Measures

### Password Policy
```csharp
public class PasswordPolicy
{
    public static readonly PasswordOptions DefaultOptions = new()
    {
        RequiredLength = 8,
        RequiredUniqueChars = 4,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
        RequireNonAlphanumeric = true
    };
}

public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
{
    public async Task<IdentityResult> ValidateAsync(
        UserManager<ApplicationUser> manager, 
        ApplicationUser user, 
        string password)
    {
        var errors = new List<IdentityError>();

        // Check for common passwords
        if (IsCommonPassword(password))
        {
            errors.Add(new IdentityError
            {
                Code = "CommonPassword",
                Description = "Please choose a less common password."
            });
        }

        // Check for personal information
        if (ContainsPersonalInformation(password, user))
        {
            errors.Add(new IdentityError
            {
                Code = "PersonalInfoPassword",
                Description = "Password cannot contain personal information."
            });
        }

        return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }
}
```

### Rate Limiting
```csharp
public class AuthenticationRateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AuthenticationRateLimitMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var isAuthEndpoint = endpoint?.Metadata?.GetMetadata<RateLimitAttribute>() != null;

        if (isAuthEndpoint)
        {
            var clientId = GetClientId(context);
            var cacheKey = $"auth_rate_limit_{clientId}";
            
            var attempts = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                return 0;
            });

            if (attempts >= 5)
            {
                _logger.LogWarning("Rate limit exceeded for client {ClientId}", clientId);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            _cache.Set(cacheKey, attempts + 1);
        }

        await _next(context);
    }
}
```

### Two-Factor Authentication
```csharp
public class TwoFactorAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;

    public async Task<string> GenerateTwoFactorCodeAsync(ApplicationUser user)
    {
        var code = GenerateRandomCode();
        await _userManager.SetTwoFactorEnabledAsync(user, true);
        
        // Store code securely with expiration
        await StoreTwoFactorCodeAsync(user.Id, code);
        
        return code;
    }

    public async Task SendTwoFactorCodeAsync(ApplicationUser user, string code)
    {
        if (!string.IsNullOrEmpty(user.Email))
        {
            await _emailSender.SendEmailAsync(user.Email, "Your verification code", 
                $"Your verification code is: {code}");
        }

        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            await _smsSender.SendSmsAsync(user.PhoneNumber, $"Your verification code is: {code}");
        }
    }
}
```

## Session Management

### Refresh Token Strategy
```csharp
public class RefreshTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMemoryCache _cache;

    public async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = GenerateSecureToken();
        var expiryTime = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiryTime;
        await _userManager.UpdateAsync(user);

        // Cache for quick validation
        _cache.Set($"refresh_token_{user.Id}", refreshToken, expiryTime);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var cachedToken = _cache.Get<string>($"refresh_token_{userId}");
        if (cachedToken != refreshToken) return false;

        return user.RefreshToken == refreshToken && 
               user.RefreshTokenExpiryTime > DateTime.UtcNow;
    }
}
```

This authentication and authorization system provides:

1. **Secure JWT-based authentication** with refresh tokens
2. **Permission-based access control** tied to subscription levels
3. **Family member access control** for shared resources
4. **Rate limiting** to prevent brute force attacks
5. **Two-factor authentication** support
6. **Password security policies** and validation
7. **Comprehensive audit logging** and security monitoring

The system ensures that users can only access features appropriate for their subscription level while maintaining security best practices throughout the application.