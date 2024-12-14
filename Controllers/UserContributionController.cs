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
#pragma warning disable CS8604 // Possible null reference argument.
            var userContributions = await _context.Contributions
                .Where(c => c.UserId == userId)
                .Include(c => c.Project)
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.
            return View(userContributions);
        }

        // GET: UserContributionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserContributionController/Create
        public ActionResult Create(int projectId)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var project =  _context.Projects.FindAsync(projectId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (project == null)
            {
                return NotFound();
            }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

            var contribution = new Contribution
            {
                ProjectId = projectId
            };
            //ViewBag.ProjectId = projectId;

            return View(contribution);
        }

        // POST: UserContributionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contribution contribution)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contribution.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    contribution.ContributionDate = DateTime.Now;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    _context.Contributions.Add(contribution);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "UserProject");
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
#pragma warning disable CS8602 // Possible null reference argument.
            var contribution = await _context.Contributions.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
                    var existingContribution = await _context.Contributions.FindAsync(id);
                    if (existingContribution == null)
                    {
                        return NotFound();
                    }

                    existingContribution.Amount = contribution.Amount;

                    _context.Update(existingContribution);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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

#pragma warning disable CS8604 // Possible null reference argument.
            var contribution = await _context.Contributions
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
#pragma warning restore CS8604 // Possible null reference argument.
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
