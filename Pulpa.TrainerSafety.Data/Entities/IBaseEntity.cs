
namespace Pulpa.TrainerSafety.Data.Entities
{
    public interface IBaseEntity
    {
        DateTime? DateCreated { get; set; }
        DateTime? DateDeleted { get; set; }
        DateTime? DateUpdated { get; set; }
        string? DeletedBy { get; set; }
        string? UserCreated { get; set; }
        string? UserUpdated { get; set; }
    }
}