using System.Diagnostics;
using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    public class HomeController : Controller
    {
        private CrowdFundingDbContext _context { get; set; }

        public HomeController(CrowdFundingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
 
            var projects = await _context.Projects
                                .Include(p => p.User)
                                .Include(p => p.Category)
                                .Where(p => p.User != null && p.Category != null).ToListAsync();

            return View(projects);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
 
            var project = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
