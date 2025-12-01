using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pulpa.TrainerSafety.Data.Entities;

namespace Pulpa.TrainerSafety.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserTrainerSafety>
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
                 
            });

            builder.Entity<FamilyGroup>(entity =>
            {
                entity.HasOne(fg => fg.Usuario)
                      .WithMany()
                      .HasForeignKey("OwnerId")
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(fg => fg.Members)
                      .WithOne()
                      .HasForeignKey("FamilyGroupId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.HasDefaultSchema("TrainerSafety");
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<FamilyGroup> FamilyGroup { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<CampaignTarget> CampaignTarget { get; set; }
        public DbSet<CampaignResult> CampaignResult { get; set; }
        public DbSet<PhishingTemplate> PhishingTemplate { get; set; }
        public DbSet<EducationalContent> EducationalContent { get; set; }
    }
}
