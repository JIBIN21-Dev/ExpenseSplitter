namespace ExpenseSplitter.Models
{
    public class SiteVisit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public int VisitCount { get; set; } = 1;
    }
}
