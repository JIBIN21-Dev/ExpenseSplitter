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
            // TEMPORARILY DISABLED: Visitor tracking is causing database errors
            // We'll fix the SiteVisits table later

            // Set default value for now
            ViewBag.TotalVisits = 0;

            /* COMMENTED OUT UNTIL DATABASE IS FIXED
            try
            {
                var today = DateTime.UtcNow.Date;
                var allVisits = _context.SiteVisits.ToList();
                var visit = allVisits.FirstOrDefault(v => v.VisitDate.Date == today);

                if (visit == null)
                {
                    visit = new SiteVisit { VisitDate = today, VisitCount = 1 };
                    _context.SiteVisits.Add(visit);
                    _context.SaveChanges();
                }
                else
                {
                    visit.VisitCount++;
                    _context.SiteVisits.Update(visit);
                    _context.SaveChanges();
                }

                ViewBag.TotalVisits = _context.SiteVisits.Sum(v => v.VisitCount);
            }
            catch (Exception ex)
            {
                ViewBag.TotalVisits = 0;
                Console.WriteLine($"Visitor tracking error: {ex.Message}");
            }
            */

            return View();
        }
    }
}