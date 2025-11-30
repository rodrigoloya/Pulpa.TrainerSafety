using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class BaseEntity
    {       
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
        public string? DeletedBy { get; set; }

    }
    public class Usuario 
    {
        public string UsuarioExternalId { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int? FamilyGroupId { get; set; }
        public virtual FamilyGroup? FamilyGroup { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CampaignResult> CampaignResults { get; set; }
    }

    public enum SubscriptionType
    {
        Free = 0,
        Pro = 1
    }

    public class FamilyGroup : BaseEntity
    {
        public int FamilyGroupId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public virtual Usuario Owner { get; set; }
        public virtual ICollection<Usuario> Members { get; set; }
    }

    public class Campaign : BaseEntity
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public virtual Usuario Owner { get; set; }
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
    public class PhishingTemplate : BaseEntity
    {
        public int PhishingTemplateId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public string SmsContent { get; set; }
        public string LandingPageUrl { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        [MaxLength(255)]
        public string Category { get; set; }
        public bool IsCustom { get; set; }
        public string? CreatedById { get; set; }
        public virtual Usuario? CreatedBy { get; set; }
    }

    public enum DifficultyLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
        Expert = 3
    }
    public class CampaignTarget : BaseEntity
    {
        public int CampaignTargetId { get; set; }
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int TargetUserId { get; set; }
        public virtual Usuario TargetUser { get; set; }
        public string EmailAddress { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public DateTime SentAt { get; set; }
        public bool EmailSent { get; set; }
        public bool SmsSent { get; set; }
    }

    public class CampaignResult : BaseEntity
    {
        public int CampaignResultId { get; set; }
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
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
    public class EducationalContent : BaseEntity
    {
        public int EducationalContentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

}
