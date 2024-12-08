using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRewardController/Create
        public ActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username");
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
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", userReward.UserId);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRewardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRewardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRewardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
