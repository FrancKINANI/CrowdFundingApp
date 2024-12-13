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
    [Authorize(Roles ="Admin, User")]
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

        // GET: UserContributionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserContributionController/Create
        [HttpGet]
        public ActionResult Create(int projectId)
        {
            var project =  _context.Projects.FindAsync(projectId);
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
        public async Task<IActionResult> Create(Contribution contribution)
        {
            Console.WriteLine("Méthode POST appelée");
            try
            {
                if (ModelState.IsValid)
                {
                    contribution.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    contribution.ContributionDate = DateTime.Now;

                    _context.Contributions.Add(contribution);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Contribution enregistrée avec succès.");
                    return RedirectToAction("Index", "Projects");
                }

                Console.WriteLine("ModelState invalide");
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserContributionController/Edit/5
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

        // GET: UserContributionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserContributionController/Delete/5
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
