using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CrowdFundingApp.Controllers
{
    public class UserControllerReward : Controller
    {
        private readonly CrowdFundingDbContext _context;

        public UserControllerReward(CrowdFundingDbContext context)
        {
            _context = context;
        }

        // GET: UserControllerReward
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRewards = await _context.UserRewards
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Reward)
                .ToListAsync();
            return View(userRewards);
        }

        // GET: UserControllerReward/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
