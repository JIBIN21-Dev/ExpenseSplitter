namespace ExpenseSplitter.ViewModels
{
    public class CreatePoolViewModel
    {
        public string Title { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<string> MemberEmails { get; set; } = new List<string>();
    }
}