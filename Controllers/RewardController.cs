using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RewardController : Controller
    {
        public CrowdFundingDbContext _context {  get; set; }
        public RewardController(CrowdFundingDbContext context)
        {
            _context = context;
        }

        // GET: RewardController
        public ActionResult Index()
        {
            var rewards = _context.Rewards.Include(p => p.Project).ToList();
            return View(rewards);
        }

        // GET: RewardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RewardController/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title");
            return View();
        }

        // POST: RewardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("RewardId,Description,MinimumContribution,ProjectId")] Reward reward)
        {
            try
            {
                _context.Add(reward);
                await _context.SaveChangesAsync();
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", reward.ProjectId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RewardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RewardController/Edit/5
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

        // GET: RewardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RewardController/Delete/5
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
