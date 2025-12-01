using Pulpa.TrainerSafety.Domain;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class Usuario : BaseEntity, IBaseEntity
    {
        public int UsuarioId { get; set; }
        public string? UsuarioExternalId { get; set; }              
        public SubscriptionType SubscriptionType { get; set; }        
        public virtual FamilyGroup? FamilyGroup { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CampaignResult> CampaignResults { get; set; }
        public bool EnableNotifications { get; set; }
        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Unknown = 0,
        Inactive,
        Active,
        Suspended,
        PastDue,
        Cancelled
    }
}
