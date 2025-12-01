namespace Pulpa.TrainerSafety.Data.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
        public string? DeletedBy { get; set; }

    }

}
