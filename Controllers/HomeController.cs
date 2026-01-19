// Home page with visitor counter
using Microsoft.AspNetCore.Mvc;
using ExpenseSplitter.Data;
using ExpenseSplitter.Models;

namespace ExpenseSplitter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Track visitor count
            var today = DateTime.UtcNow.Date;
            var visit = _context.SiteVisits.FirstOrDefault(v => v.VisitDate == today);

            if (visit == null)
            {
                visit = new SiteVisit { VisitDate = today, VisitCount = 1 };
                _context.SiteVisits.Add(visit);
            }
            else
            {
                visit.VisitCount++;
            }

            _context.SaveChanges();

            // Get total visits
            var totalVisits = _context.SiteVisits.Sum(v => v.VisitCount);
            ViewBag.TotalVisits = totalVisits;

            return View();
        }
    }
}