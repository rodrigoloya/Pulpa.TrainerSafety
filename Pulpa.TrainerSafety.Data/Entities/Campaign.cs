namespace Pulpa.TrainerSafety.Data.Entities
{
    public class Campaign : BaseEntity, IBaseEntity
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }       
        public virtual Usuario? Usuario { get; set; }
        public CampaignType Type { get; set; }
        public CampaignStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public PhishingTemplate? PhishingTemplate { get; set; }
        public virtual ICollection<CampaignTarget> CampaignTargets { get; set; }
        public virtual ICollection<CampaignResult> CampaignResults { get; set; }
    }
    public enum CampaignType
    {
        Unknown = 0,
        Email,
        SMS,
        Both
    }

    public enum CampaignStatus
    {
        Unknown = 0,
        Draft,
        Scheduled,
        Active,
        Completed,
        Cancelled
    }

}
