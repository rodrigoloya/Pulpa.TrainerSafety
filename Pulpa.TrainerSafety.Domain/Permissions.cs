namespace Pulpa.TrainerSafety.Domain;

public static class Permissions
{
    public const string ReadUsers = "user:read";
    public const string EditUsers = "user:update";
    public const string DeleteUsers = "user:delete";
    public const string CreateUsers = "user:create";

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

    public static readonly Dictionary<SubscriptionType, string[]> SubscriptionPermissionsMap = new()
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

public enum SubscriptionType
{
    Unknown = 0,
    Free,
    Pro
}
