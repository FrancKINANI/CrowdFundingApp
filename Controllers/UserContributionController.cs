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
         private readonly CrowdFundingDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserContributionController> _logger;

        public UserContributionController(
            CrowdFundingDbContext context, 
            UserManager<User> userManager,
            ILogger<UserContributionController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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

        // GET: UserContributionController/Create
        public ActionResult Create(int projectId)
        {
            var project = _context.Projects.FindAsync(projectId);
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
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Create(Contribution contribution)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     contribution.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                     contribution.ContributionDate = DateTime.Now;

                     _context.Contributions.Add(contribution);
                     var project = _context.Projects.Find(contribution.ProjectId);
                     if (project != null)
                     {
                         project.CurrentAmount += contribution.Amount;
                         _context.Update(project);
                         _context.SaveChanges();
                         return RedirectToAction(nameof(Index));
                     }
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
         }*/

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contribution contribution)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get current user and project
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var project = await _context.Projects
                        .Include(p => p.User)
                        .FirstOrDefaultAsync(p => p.ProjectId == contribution.ProjectId);

                    // Validate project exists
                    if (project == null)
                    {
                        ModelState.AddModelError("", "Project not found.");
                        return View(contribution);
                    }

                    // Check if project is still accepting investments
                    if (project.EndDate < DateTime.Now)
                    {
                        ModelState.AddModelError("", "This project's funding period has ended.");
                        return View(contribution);
                    }

                    // Check if investment would exceed goal
                    if (project.CurrentAmount + contribution.Amount > project.GoalAmount)
                    {
                        ModelState.AddModelError("", "This investment would exceed the project's goal amount.");
                        return View(contribution);
                    }

                    // Create the contribution
                    contribution.UserId = userId;
                    contribution.ContributionDate = DateTime.Now;
                    _context.Contributions.Add(contribution);

                    // Update project amount
                    project.CurrentAmount += contribution.Amount;
                    _context.Update(project);

                    // Check if this contribution meets any reward thresholds
                    var eligibleRewards = await _context.Rewards
                        .Where(r => r.ProjectId == project.ProjectId && r.MinimumContribution <= contribution.Amount)
                        .ToListAsync();

                    foreach (var reward in eligibleRewards)
                    {
                        var userReward = new UserReward
                        {
                            UserId = userId,
                            RewardId = reward.RewardId,
                            DateAwarded = DateTime.Now
                        };
                        _context.UserRewards.Add(userReward);
                    }

                    await _context.SaveChangesAsync();

                    // Send notification to project owner
                    // You can implement this later with SignalR or email notifications

                    return RedirectToAction("Details", "Project", new { id = project.ProjectId });
                }
                return View(contribution);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating contribution: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while processing your investment.");
                return View(contribution);
            }
        }
        //
        [HttpGet]
public async Task<IActionResult> Statistics()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
    var stats = new ContributionStatistics
    {
        TotalInvested = await _context.Contributions
            .Where(c => c.UserId == userId)
            .SumAsync(c => c.Amount),
            
        ProjectsInvested = await _context.Contributions
            .Where(c => c.UserId == userId)
            .Select(c => c.ProjectId)
            .Distinct()
            .CountAsync(),
            
        RewardsEarned = await _context.UserRewards
            .Where(ur => ur.UserId == userId)
            .CountAsync(),
            
        ActiveInvestments = await _context.Contributions
            .Include(c => c.Project)
            .Where(c => c.UserId == userId && c.Project.EndDate > DateTime.Now)
            .ToListAsync()
    };
    
    return View(stats);
}
        //

        // GET: UserContributionController/Edit/5
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

            var contribution = await _context.Contributions
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.ContributionId == id);
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
            var contribution = await _context.Contributions.FindAsync(id);
            _context.Contributions.Remove(contribution);
            var project = await _context.Projects.FindAsync(contribution.ProjectId);
            if (project != null)
            {
                project.CurrentAmount -= contribution.Amount;
                _context.Update(project);
                _context.Contributions.Remove(contribution);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
            return _context.Contributions.Any(e => e.ContributionId == id);
        }

        //

    }
}
