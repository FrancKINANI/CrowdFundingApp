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
    //[Authorize(Roles ="User")]
    public class UserProjectController : Controller
    {
        public readonly CrowdFundingDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserProjectController(CrowdFundingDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: UserProjectConsoller
        public async Task<ActionResult> Index()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var projects = await _context.Projects
            //    .Where(c => c.UserId == userId)
            //    .ToListAsync();
            //return View(projects);
            var projects = await _context.Projects
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToListAsync();
            return View(projects);
        }

        // GET: UserProjectConsoller/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserProjectConsoller/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: UserProjectConsoller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    project.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Projects.Add(project);
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", project.CategoryId);
                    await _context.SaveChangesAsync();
                    return View();
                }
                return View(project);
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProjectConsoller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProjectConsoller/Edit/5
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

        // GET: UserProjectConsoller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProjectConsoller/Delete/5
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
