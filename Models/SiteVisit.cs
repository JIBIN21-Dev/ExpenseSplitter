using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseSplitter.Models
{
    public class SiteVisit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.UtcNow.Date;

        public int VisitCount { get; set; } = 1;
    }
}