using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

            builder.Entity<UserTrainerSafety>(entity =>
            {
                entity.Property(e=> e.EnableNotifications).HasDefaultValue(true);
                entity.HasOne(e => e.FamilyGroup)
                    .WithMany()
                    .HasForeignKey(e => e.FamilyGroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.HasDefaultSchema("TrainerSafety");
        }

        public DbSet<UserTrainerSafety> UserTrainerSafeties { get; set; }
        public DbSet<FamilyGroup> FamilyGroups { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignTarget> CampaignTargets { get; set; }
        public DbSet<CampaignResult> CampaignResults { get; set; }
        public DbSet<PhishingTemplate> PhishingTemplate { get; set; }

        public DbSet<EducationalContent> EducationalContent { get; set; }

    }

}
