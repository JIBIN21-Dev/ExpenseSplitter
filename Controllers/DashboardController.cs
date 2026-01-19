// User's personal dashboard showing their dues
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseSplitter.Data;
using ExpenseSplitter.Models;

namespace ExpenseSplitter.Controllers
{
    [Authorize] // Must be logged in to access
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Get pools where user is a member
            var memberPools = await _context.PoolMembers
                .Include(pm => pm.Pool)
                .ThenInclude(p => p.Creator)
                .Where(pm => pm.UserId == user.Id)
                .ToListAsync();

            // Get pools created by user
            var createdPools = await _context.SplitPools
                .Include(p => p.Members)
                .ThenInclude(m => m.User)
                .Where(p => p.CreatorId == user.Id)
                .ToListAsync();

            ViewBag.MemberPools = memberPools;
            ViewBag.CreatedPools = createdPools;

            return View();
        }

        // Mark payment as done
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int poolMemberId)
        {
            var poolMember = await _context.PoolMembers.FindAsync(poolMemberId);

            if (poolMember != null)
            {
                poolMember.AmountPaid = poolMember.AmountDue;
                poolMember.IsCleared = true;
                poolMember.PaidAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Check if all members paid
                var pool = await _context.SplitPools
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == poolMember.PoolId);

                if (pool != null && pool.Members.All(m => m.IsCleared))
                {
                    pool.IsCompleted = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}