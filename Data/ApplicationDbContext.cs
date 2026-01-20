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
        // REMOVED: public DbSet<SiteVisit> SiteVisits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // REMOVED: SiteVisit configuration

            // Configure PoolMember relationships
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