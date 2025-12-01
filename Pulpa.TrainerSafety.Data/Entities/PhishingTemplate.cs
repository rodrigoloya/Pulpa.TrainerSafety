using System.ComponentModel.DataAnnotations;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class PhishingTemplate : BaseEntity, IBaseEntity
    {
        public int PhishingTemplateId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? BodyContent { get; set; }
        public string? SmsContent { get; set; }
        public string? LandingPageUrl { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        [MaxLength(255)]
        public string? Category { get; set; }
        public bool IsCustom { get; set; }     
       
    }

}
