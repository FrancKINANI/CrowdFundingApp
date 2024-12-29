using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CrowdFundingApp.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly CrowdFundingDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectController(
            CrowdFundingDbContext context,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Controllers/ProjectController.cs - Additional Actions
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Contributions)
                .Include(p => p.Rewards)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Startup")]
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Startup")]
        public async Task<ActionResult> Create([Bind("Title,Description,GoalAmount,StartDate,EndDate,CategoryId")] Project project, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    project.UserId = user.Id;
                    project.Status = ProjectStatus.Draft;
                    project.CreatedAt = DateTime.Now;
                    project.CurrentAmount = 0;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "project-images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        project.ImageUrl = "/project-images/" + uniqueFileName;
                    }

                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating project: " + ex.Message);
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", project.CategoryId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Startup,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && project.UserId != user.Id)
            {
                return Forbid();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", project.CategoryId);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Startup,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title,Description,GoalAmount,StartDate,EndDate,CategoryId,Status")] Project project, IFormFile? imageFile)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            var existingProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectId == id);
            if (existingProject == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && existingProject.UserId != user.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.UserId = existingProject.UserId;
                    project.CurrentAmount = existingProject.CurrentAmount;
                    project.ImageUrl = existingProject.ImageUrl;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingProject.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingProject.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "project-images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        project.ImageUrl = "/project-images/" + uniqueFileName;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", project.CategoryId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Startup,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, ProjectStatus status)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && project.UserId != user.Id)
            {
                return Forbid();
            }

            project.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = project.ProjectId });
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
