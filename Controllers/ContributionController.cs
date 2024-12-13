using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin, User")]
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
        public ActionResult Details(int id)
        {
            return View();
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
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContributionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContributionController/Edit/5
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

        // GET: ContributionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContributionController/Delete/5
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
