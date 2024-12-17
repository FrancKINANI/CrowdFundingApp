using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class UserContributionController : Controller
    {
        public readonly CrowdFundingDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserContributionController(CrowdFundingDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserContributionController
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userContributions = await _context.Contributions
                .Where(c => c.UserId == userId)
                .Include(c => c.Project)
                .ToListAsync();
            return View(userContributions);
        }

        // GET: UserContributionController/Create
        public ActionResult Create(int projectId)
        {
            var project = _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }
            var contribution = new Contribution
            {
                ProjectId = projectId
            };

            return View(contribution);
        }

        // POST: UserContributionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contribution contribution)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contribution.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    contribution.ContributionDate = DateTime.Now;

                    _context.Contributions.Add(contribution);
                    var project = _context.Projects.Find(contribution.ProjectId);
                    if (project != null)
                    {
                        project.CurrentAmount += contribution.Amount;
                        _context.Update(project);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(contribution);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return View(contribution);
            }
        }

        // GET: UserContributionController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }
            return View(contribution);
        }

        // POST: UserContributionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributionId, Amount")] Contribution contribution)
        {
            if (id != contribution.ContributionId)
            {
                return NotFound(); 
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingContribution = await _context.Contributions.AsNoTracking()
                        .FirstOrDefaultAsync(c => c.ContributionId == id);

                    if (existingContribution != null)
                    {
                        var project = await _context.Projects.FindAsync(contribution.ProjectId);
                        if (project != null)
                        {
                            var difference = contribution.Amount - existingContribution.Amount;
                            project.CurrentAmount += difference;

                            _context.Update(project);
                        }

                        _context.Update(contribution);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributionExists(contribution.ContributionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(contribution);
        }

        // GET: UserContributionController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // POST: UserContributionController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            _context.Contributions.Remove(contribution);
            var project = await _context.Projects.FindAsync(contribution.ProjectId);
            if (project != null)
            {
                project.CurrentAmount -= contribution.Amount;
                _context.Update(project);
                _context.Contributions.Remove(contribution);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
            return _context.Contributions.Any(e => e.ContributionId == id);
        }
    }
}
