namespace ExpenseSplitter.Models
{
    public class PoolMember
    {
        public int Id { get; set; }
        public int PoolId { get; set; }
        public SplitPool Pool { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public decimal AmountDue { get; set; } // How much they owe
        public decimal AmountPaid { get; set; } = 0; // How much they've paid
        public bool IsCleared { get; set; } = false; // Fully paid?
        public DateTime? PaidAt { get; set; } // When they paid
    }
}
