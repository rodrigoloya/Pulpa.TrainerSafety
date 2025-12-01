namespace Pulpa.TrainerSafety.Data.Entities
{
    public class EducationalContent : BaseEntity, IBaseEntity
    {
        public int EducationalContentId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public string? VideoUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public enum DifficultyLevel
    {
        Unknown = 0,
        Easy,
        Medium,
        Hard,
        Expert,
    }

}
