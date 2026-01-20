using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseSplitter.Models;

namespace ExpenseSplitter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SplitPool> SplitPools { get; set; }
        public DbSet<PoolMember> PoolMembers { get; set; }
        public DbSet<SiteVisit> SiteVisits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SiteVisit
            modelBuilder.Entity<SiteVisit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd(); // Ensure auto-increment
                entity.Property(e => e.VisitDate)
                    .HasColumnType("date"); // Store as date only
                entity.HasIndex(e => e.VisitDate)
                    .IsUnique(); // Ensure one record per date
            });

            // Configure other relationships as needed
            modelBuilder.Entity<PoolMember>()
                .HasOne(pm => pm.Pool)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.PoolId);

            modelBuilder.Entity<PoolMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.PoolMemberships)
                .HasForeignKey(pm => pm.UserId);

            modelBuilder.Entity<SplitPool>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.CreatedPools)
                .HasForeignKey(p => p.CreatorId);
        }
    }
}