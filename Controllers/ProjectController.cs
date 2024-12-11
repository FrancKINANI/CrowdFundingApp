using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CrowdFundingApp.Controllers
{
    //[Authorize(Roles = "Admin, User")]
    public class ProjectController : Controller
    {
        public CrowdFundingDbContext _context { get; set; }
        public ProjectController(CrowdFundingDbContext context)
        {
            _context = context;
        }

        // GET: ProjectController
        public ActionResult Index()
        {
            var projects = _context.Projects.Include(p => p.User).Include(p => p.Category);
            return View(projects.ToList());
        }

        // GET: ProjectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectController/Create
        public ActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: ProjectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ProjectId,Title,Description,GoalAmount,CurrentAmount,StartDate,EndDate,UserId,CategoryId")] Project project)
        {
            try
            {
                _context.Projects.Add(project);        
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", project.UserId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", project.CategoryId);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectController/Edit/5
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

        // GET: ProjectController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectController/Delete/5
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
