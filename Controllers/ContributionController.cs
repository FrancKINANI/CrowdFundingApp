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
#pragma warning disable CS8604 // Possible null reference argument.
            var contributions = _context.Contributions.Include(c => c.User).Include(c => c.Project).ToList();
#pragma warning restore CS8604 // Possible null reference argument.
            return View(contributions);
        }

        // GET: ContributionController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
#pragma warning disable CS8604 // Possible null reference argument.
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _context.Contributions.Add(contribution);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", contribution.UserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", contribution.ProjectId);
                _context.SaveChanges();
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
#pragma warning disable CS8602 // Possible null reference argument.
            var contribution = await _context.Contributions.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
                    contribution.ContributionDate = DateTime.Now;
                    _context.Update(contribution);
                    await _context.SaveChangesAsync();
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

#pragma warning disable CS8604 // Possible null reference argument.
            var contribution = await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
#pragma warning restore CS8604 // Possible null reference argument.
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var contribution = await _context.Contributions.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            _context.Contributions.Remove(contribution);
#pragma warning restore CS8604 // Possible null reference argument.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return _context.Contributions.Any(e => e.ContributionId == id);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
