using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRewardController : Controller
    {
        public CrowdFundingDbContext _context { get; set; }
        public UserRewardController(CrowdFundingDbContext context)
        {
            _context = context;
        }

        // GET: UserRewardController
        public ActionResult Index()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            var userRewards = _context.UserRewards.Include(ur => ur.User).Include(ur => ur.Reward).ThenInclude(r => r.Project);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return View(userRewards.ToList());
        }

        // GET: UserRewardController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var userReward = await _context.UserRewards
                .Include(ur => ur.User)
                .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
                .FirstOrDefaultAsync(m => m.UserRewardId == id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
            if (userReward == null)
            {
                return NotFound();
            }

            return View(userReward);
        }

        // GET: UserRewardController/Create
        public ActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
#pragma warning disable CS8604 // Possible null reference argument.
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description");
#pragma warning restore CS8604 // Possible null reference argument.
            return View();
        }

        // POST: UserRewardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("UserRewardId,UserId,RewardId,DateAwarded")] UserReward userReward)
        {
            try
            {             
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _context.UserRewards.Add(userReward);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userReward.UserId);
#pragma warning disable CS8604 // Possible null reference argument.
                ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);
#pragma warning restore CS8604 // Possible null reference argument.
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRewardController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var userReward = await _context.UserRewards.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (userReward == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userReward.UserId);
#pragma warning disable CS8604 // Possible null reference argument.
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);
#pragma warning restore CS8604 // Possible null reference argument.
            return View(userReward);
        }

        // POST: UserRewardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserRewardId,UserId,RewardId,DateAwarded")] UserReward userReward)
        {
            if (id != userReward.UserRewardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userReward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRewardExists(userReward.UserRewardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userReward.UserId);
#pragma warning disable CS8604 // Possible null reference argument.
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);
#pragma warning restore CS8604 // Possible null reference argument.
            return View(userReward);
        }

        // GET: UserRewardController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var userReward = await _context.UserRewards
                .Include(ur => ur.User)
                .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
                .FirstOrDefaultAsync(m => m.UserRewardId == id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
            if (userReward == null)
            {
                return NotFound();
            }

            return View(userReward);
        }

        // POST: UserRewardController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var userReward = await _context.UserRewards.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            _context.UserRewards.Remove(userReward);
#pragma warning restore CS8604 // Possible null reference argument.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRewardExists(int id)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return _context.UserRewards.Any(e => e.UserRewardId == id);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
