using System.ComponentModel.DataAnnotations;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class CampaignTarget : BaseEntity, IBaseEntity
    {
        public int CampaignTargetId { get; set; }      
        public virtual Campaign Campaign { get; set; }        
        public virtual Usuario? Usuario { get; set; }
        public string? EmailAddress { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        public DateTime? SentAt { get; set; }
        public bool? EmailSent { get; set; }
        public bool? SmsSent { get; set; }
    }

}
