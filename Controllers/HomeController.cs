// Home page with visitor counter
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // Use DateOnly comparison or EF.Functions for proper SQL translation
            var visit = _context.SiteVisits
                .FirstOrDefault(v => EF.Functions.DateDiffDay(v.VisitDate, DateTime.UtcNow) == 0);

            if (visit == null)
            {
                visit = new SiteVisit { VisitDate = DateTime.UtcNow.Date, VisitCount = 1 };
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