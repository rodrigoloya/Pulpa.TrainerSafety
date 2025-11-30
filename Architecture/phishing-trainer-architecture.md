# Phishing Trainer Application Architecture

## Overview
A Progressive Web Application (PWA) built with Blazor WebAssembly that provides anti-phishing training campaigns for non-technical users through simulated email/SMS attacks.

## Technology Stack
- **Frontend**: Blazor WebAssembly, C#
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **PWA**: Blazor PWA template
- **Deployment**: Azure App Service or similar

## Database Schema & Entities

### Core Entities

#### User
```csharp
public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public string? FamilyGroupId { get; set; }
    public virtual FamilyGroup? FamilyGroup { get; set; }
    public virtual ICollection<Campaign> Campaigns { get; set; }
    public virtual ICollection<CampaignResult> CampaignResults { get; set; }
}

public enum SubscriptionType
{
    Free = 0,
    Pro = 1
}
```

#### FamilyGroup
```csharp
public class FamilyGroup
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public virtual User Owner { get; set; }
    public virtual ICollection<User> Members { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### Campaign
```csharp
public class Campaign
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public virtual User Owner { get; set; }
    public CampaignType Type { get; set; }
    public CampaignStatus Status { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public PhishingTemplate Template { get; set; }
    public virtual ICollection<CampaignTarget> Targets { get; set; }
    public virtual ICollection<CampaignResult> Results { get; set; }
}

public enum CampaignType
{
    Email = 0,
    SMS = 1,
    Both = 2
}

public enum CampaignStatus
{
    Draft = 0,
    Scheduled = 1,
    Active = 2,
    Completed = 3,
    Cancelled = 4
}
```

#### PhishingTemplate
```csharp
public class PhishingTemplate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Subject { get; set; }
    public string BodyContent { get; set; }
    public string SmsContent { get; set; }
    public string LandingPageUrl { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public string Category { get; set; }
    public bool IsCustom { get; set; }
    public string? CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
}

public enum DifficultyLevel
{
    Easy = 0,
    Medium = 1,
    Hard = 2,
    Expert = 3
}
```

#### CampaignTarget
```csharp
public class CampaignTarget
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; }
    public string TargetUserId { get; set; }
    public virtual User TargetUser { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime SentAt { get; set; }
    public bool EmailSent { get; set; }
    public bool SmsSent { get; set; }
}
```

#### CampaignResult
```csharp
public class CampaignResult
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime EmailOpenedAt { get; set; }
    public DateTime? LinkClickedAt { get; set; }
    public DateTime? DataSubmittedAt { get; set; }
    public string? ClickedLinkUrl { get; set; }
    public string? SubmittedData { get; set; }
    public ResultType Result { get; set; }
    public int Score { get; set; }
}

public enum ResultType
{
    NotDelivered = 0,
    DeliveredNotOpened = 1,
    OpenedNoClick = 2,
    ClickedNoSubmit = 3,
    SubmittedData = 4,
    ReportedAsPhishing = 5
}
```

#### EducationalContent
```csharp
public class EducationalContent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
```

## API Architecture

### Main Controllers

#### Authentication Controller
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/logout`
- `GET /api/auth/profile`
- `PUT /api/auth/profile`

#### Campaign Management Controller
- `GET /api/campaigns`
- `POST /api/campaigns`
- `GET /api/campaigns/{id}`
- `PUT /api/campaigns/{id}`
- `DELETE /api/campaigns/{id}`
- `POST /api/campaigns/{id}/start`
- `POST /api/campaigns/{id}/stop`

#### Template Controller
- `GET /api/templates`
- `GET /api/templates/{id}`
- `POST /api/templates` (for custom templates - Pro users only)

#### Analytics Controller
- `GET /api/analytics/dashboard`
- `GET /api/analytics/campaigns/{id}/results`
- `GET /api/analytics/users/{id}/progress`

#### Subscription Controller
- `GET /api/subscription/current`
- `POST /api/subscription/upgrade`
- `POST /api/subscription/cancel`

#### Family Management Controller
- `GET /api/family/groups`
- `POST /api/family/groups`
- `POST /api/family/groups/{id}/members`
- `DELETE /api/family/groups/{id}/members/{userId}`

## Frontend Component Structure

### Main Layout Components
- `MainLayout.razor` - Root layout with navigation
- `NavMenu.razor` - Navigation sidebar
- `LoginDisplay.razor` - User authentication status

### Page Components
- `Pages/Index.razor` - Dashboard
- `Pages/Campaigns/CampaignList.razor` - Campaign listing
- `Pages/Campaigns/CreateCampaign.razor` - Campaign creation wizard
- `Pages/Campaigns/CampaignDetail.razor` - Campaign details and results
- `Pages/Templates/TemplateList.razor` - Template browsing
- `Pages/Analytics/Dashboard.razor` - Analytics dashboard
- `Pages/Education/LearningCenter.razor` - Educational content
- `Pages/Account/Profile.razor` - User profile management
- `Pages/Account/Subscription.razor` - Subscription management
- `Pages/Family/GroupManagement.razor` - Family group management

### Shared Components
- `Components/CampaignCard.razor` - Campaign summary card
- `Components/ResultChart.razor` - Campaign results visualization
- `Components/ProgressBar.razor` - Progress indicators
- `Components/EducationalTip.razor` - Educational content cards
- `Components/SubscriptionBanner.razor` - Upgrade prompts

## User Flow Diagrams

### Campaign Creation Flow
1. User selects "Create New Campaign"
2. Choose template (email/SMS/both)
3. Configure targets (self/family members)
4. Schedule campaign
5. Review and confirm
6. Campaign goes live

### Result Tracking Flow
1. User receives simulated phishing email/SMS
2. System tracks: email open, link clicks, data submission
3. Results updated in real-time dashboard
4. Educational feedback provided based on actions
5. Progress score calculated

## Security Considerations

### Data Protection
- Encrypt all user data at rest
- Use HTTPS for all communications
- Implement rate limiting for API endpoints
- Sanitize all user inputs
- Secure storage of credentials

### Campaign Security
- Isolate simulated phishing content
- Use dedicated domains for landing pages
- Clear branding indicating training content
- Automatic cleanup of campaign data

## PWA Features

### Offline Capabilities
- Cache essential application shell
- Store campaign data locally
- Sync when connection restored
- Basic offline dashboard viewing

### Mobile Optimization
- Touch-friendly interface
- Responsive design for all screen sizes
- Install prompt for mobile devices
- Push notifications for campaign results

## Subscription Model Implementation

### Free Tier ($0)
- 1 active campaign at a time
- Personal use only
- Basic templates only
- Limited analytics (7 days)
- No family groups

### Pro Tier ($10/month)
- Unlimited campaigns
- Up to 5 family members
- Custom templates
- Advanced analytics (90 days)
- Priority support
- Campaign scheduling

## Development Phases

### Phase 1: MVP (4-6 weeks)
- User authentication
- Basic campaign creation
- Email campaigns only
- Simple result tracking
- Basic dashboard

### Phase 2: Enhanced Features (3-4 weeks)
- SMS campaigns
- Family groups
- Educational content
- Improved analytics
- PWA features

### Phase 3: Pro Features (3-4 weeks)
- Custom templates
- Advanced reporting
- Subscription management
- Mobile app optimization

## Deployment Architecture

### Production Environment
- Load balancer
- Multiple app service instances
- SQL Azure database
- Redis cache for session management
- CDN for static assets
- Application Insights for monitoring

### Development Environment
- Local development with Docker
- Azure DevOps for CI/CD
- Staging environment for testing
- Automated testing pipeline