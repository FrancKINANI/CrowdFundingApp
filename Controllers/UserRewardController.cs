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

 
            var userRewards = _context.UserRewards.Include(ur => ur.User).Include(ur => ur.Reward).ThenInclude(r => r.Project);


            return View(userRewards.ToList());
        }

        // GET: UserRewardController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

 

            var userReward = await _context.UserRewards
                .Include(ur => ur.User)
                .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
                .FirstOrDefaultAsync(m => m.UserRewardId == id);


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
 
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description");

            return View();
        }

        // POST: UserRewardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("UserRewardId,UserId,RewardId,DateAwarded")] UserReward userReward)
        {
            try
            {             

                _context.UserRewards.Add(userReward);

                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userReward.UserId);
 
                ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);

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


            var userReward = await _context.UserRewards.FindAsync(id);

            if (userReward == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userReward.UserId);
 
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);

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
 
            ViewData["RewardId"] = new SelectList(_context.Rewards.Include(r => r.Project), "RewardId", "Description", userReward.RewardId);

            return View(userReward);
        }

        // GET: UserRewardController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

 

            var userReward = await _context.UserRewards
                .Include(ur => ur.User)
                .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
                .FirstOrDefaultAsync(m => m.UserRewardId == id);


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

            var userReward = await _context.UserRewards.FindAsync(id);

 
            _context.UserRewards.Remove(userReward);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRewardExists(int id)
        {

            return _context.UserRewards.Any(e => e.UserRewardId == id);

        }
    }
}
