// Create and manage split pools
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseSplitter.Data;
using ExpenseSplitter.Models;
using ExpenseSplitter.ViewModels;

namespace ExpenseSplitter.Controllers
{
    [Authorize]
    public class PoolController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PoolController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Pool/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Pool/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreatePoolViewModel model, string memberEmailsInput)
        {
            var user = await _userManager.GetUserAsync(User);

            // Parse member emails from textarea (one per line)
            var memberEmails = memberEmailsInput
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();

            // Validate all emails exist
            var notFoundEmails = new List<string>();
            var members = new List<ApplicationUser>();

            foreach (var email in memberEmails)
            {
                var member = await _userManager.FindByEmailAsync(email);
                if (member == null)
                {
                    notFoundEmails.Add(email);
                }
                else
                {
                    members.Add(member);
                }
            }

            if (notFoundEmails.Any())
            {
                ViewBag.Error = $"These emails are not registered: {string.Join(", ", notFoundEmails)}";
                return View(model);
            }

            if (!members.Any())
            {
                ViewBag.Error = "Please add at least one member.";
                return View(model);
            }

            // Create the pool
            var pool = new SplitPool
            {
                Title = model.Title,
                TotalAmount = model.TotalAmount,
                CreatorId = user.Id
            };

            _context.SplitPools.Add(pool);
            await _context.SaveChangesAsync();

            // Calculate amount per person
            var amountPerPerson = model.TotalAmount / members.Count;

            // Add members to pool
            foreach (var member in members)
            {
                var poolMember = new PoolMember
                {
                    PoolId = pool.Id,
                    UserId = member.Id,
                    AmountDue = amountPerPerson
                };

                _context.PoolMembers.Add(poolMember);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Pool/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pool = await _context.SplitPools
                .Include(p => p.Creator)
                .Include(p => p.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pool == null)
            {
                return NotFound();
            }

            return View(pool);
        }

        // POST: /Pool/AddExpense
        [HttpPost]
        public async Task<IActionResult> AddExpense(int poolId, decimal additionalAmount)
        {
            var pool = await _context.SplitPools
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == poolId);

            if (pool == null)
            {
                return NotFound();
            }

            // Add to total amount
            pool.TotalAmount += additionalAmount;

            // Redistribute among members
            var amountPerPerson = additionalAmount / pool.Members.Count;

            foreach (var member in pool.Members)
            {
                member.AmountDue += amountPerPerson;
                member.IsCleared = false; // Reset cleared status
            }

            pool.IsCompleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = poolId });
        }
    }
}