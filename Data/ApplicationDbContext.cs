using ExpenseSplitter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExpenseSplitter.Data
{
    // Inherits from IdentityDbContext to get all Identity tables
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Your custom tables
        public DbSet<SplitPool> SplitPools { get; set; }
        public DbSet<PoolMember> PoolMembers { get; set; }
        public DbSet<SiteVisit> SiteVisits { get; set; }

        // Configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SplitPool relationships
            builder.Entity<SplitPool>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.CreatedPools)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete pools if user deleted

            // PoolMember relationships
            builder.Entity<PoolMember>()
                .HasOne(pm => pm.Pool)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.PoolId)
                .OnDelete(DeleteBehavior.Cascade); // Delete members if pool deleted

            builder.Entity<PoolMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.PoolMemberships)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision for money
            builder.Entity<SplitPool>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<PoolMember>()
                .Property(pm => pm.AmountDue)
                .HasColumnType("decimal(18,2)");

            builder.Entity<PoolMember>()
                .Property(pm => pm.AmountPaid)
                .HasColumnType("decimal(18,2)");
        }
    }
}