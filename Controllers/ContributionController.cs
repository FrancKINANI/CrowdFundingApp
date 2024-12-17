using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContributionController : Controller
    {
        private CrowdFundingDbContext _context;

        public ContributionController(CrowdFundingDbContext context)
        {
            _context = context;
        }

        // GET: ContributionController
        public ActionResult Index()
        {
            var contributions = _context.Contributions.Include(c => c.User).Include(c => c.Project).ToList();
            return View(contributions);
        }

        // GET: ContributionController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var contribution = await _context.Contributions
#pragma warning restore CS8604 // Dereference of a possibly null reference.
                .Include(c => c.User)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // GET: ContributionController/Create
        public ActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title");
            return View();
        }

        // POST: ContributionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ContributionId,Amount,ContributionDate,UserId,ProjectId")] Contribution contribution)
        {
            try
            {

                _context.Contributions.Add(contribution);

                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", contribution.UserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", contribution.ProjectId);
                var project = _context.Projects.Find(contribution.ProjectId);
                if (project != null)
                {
                    project.CurrentAmount += contribution.Amount;
                    _context.Update(project);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContributionController/Edit/5
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", contribution.UserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", contribution.ProjectId);
            return View(contribution);
        }

        // POST: ContributionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributionId,Amount,ContributionDate,UserId,ProjectId")] Contribution contribution)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", contribution.UserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", contribution.ProjectId);
            return View(contribution);
        }

        // GET: ContributionController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contribution = await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // POST: ContributionController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            var project = await _context.Projects.FindAsync(contribution.ProjectId);
            if (project != null)
            {
                project.CurrentAmount -= contribution.Amount;
                _context.Update(project);
            }
            _context.Contributions.Remove(contribution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
            return _context.Contributions.Any(e => e.ContributionId == id);
        }
    }
}
