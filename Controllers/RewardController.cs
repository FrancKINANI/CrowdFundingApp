﻿using CrowdFundingApp.Models;
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

 
            var reward = await _context.Rewards
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RewardId == id);

            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var reward = await _context.Rewards.FindAsync(id);

            if (reward == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", reward.ProjectId);
            return View(reward);
        }

        // POST: RewardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RewardId,Description,MinimumContribution,ProjectId")] Reward reward)
        {
            if (id != reward.RewardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RewardExists(reward.RewardId))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", reward.ProjectId);
            return View(reward);
        }

        // GET: RewardController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

 
            var reward = await _context.Rewards
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RewardId == id);

            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // POST: RewardController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var reward = await _context.Rewards.FindAsync(id);

 
            _context.Rewards.Remove(reward);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RewardExists(int id)
        {
 
            return _context.Rewards.Any(e => e.RewardId == id);

        }
    }
}
