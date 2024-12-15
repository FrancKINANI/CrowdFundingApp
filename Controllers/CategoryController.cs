using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CrowdFundingDbContext _context;

        public CategoryController(CrowdFundingDbContext context)
        {
            _context = context;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            List<Category> categories = _context.Categories.ToList();
#pragma warning restore CS8604 // Possible null reference argument.
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CS8604 // Possible null reference argument.
            var category = await _context.Categories
                .Include(c => c.Projects) // Charger les projets associés
                .FirstOrDefaultAsync(m => m.CategoryId == id);
#pragma warning restore CS8604 // Possible null reference argument.

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _context.Categories.Add(category);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        { 
            if (id == null)
            {
                return NotFound();
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var category = await _context.Categories.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }


        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var category = await _context.Categories.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var category = await _context.Categories.FindAsync(id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            _context.Categories.Remove(category);
#pragma warning restore CS8604 // Possible null reference argument.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return _context.Categories.Any(e => e.CategoryId == id);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
    