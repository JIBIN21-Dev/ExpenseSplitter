namespace ExpenseSplitter.Models
{
    public class SiteVisit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow.Date;

        public int VisitCount { get; set; } = 1;
    }
}
