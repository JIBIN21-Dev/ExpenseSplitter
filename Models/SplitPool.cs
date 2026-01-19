namespace ExpenseSplitter.Models
{
    public class SplitPool
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // "Goa Trip", "Birthday Party"
        public decimal TotalAmount { get; set; } // Total money spent
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false; // All members paid?

        // Foreign key - who created this pool
        public string CreatorId { get; set; } = string.Empty;
        public ApplicationUser Creator { get; set; } = null!;

        // Navigation - all members in this pool
        public ICollection<PoolMember> Members { get; set; } = new List<PoolMember>();
    }
}
