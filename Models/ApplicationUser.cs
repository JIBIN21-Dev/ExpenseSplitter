using Microsoft.AspNetCore.Identity;

namespace ExpenseSplitter.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties beyond default Identity fields
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - relationships
        public ICollection<SplitPool> CreatedPools { get; set; } = new List<SplitPool>();
        public ICollection<PoolMember> PoolMemberships { get; set; } = new List<PoolMember>();
    }
}
