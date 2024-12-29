using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class UserRewardController : Controller
{
    private readonly CrowdFundingDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserRewardController> _logger;

    public UserRewardController(
        CrowdFundingDbContext context,
        UserManager<User> userManager,
        ILogger<UserRewardController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRewards = await _context.UserRewards
            .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
            .Where(ur => ur.UserId == userId)
            .OrderByDescending(ur => ur.DateAwarded)
            .ToListAsync();

        return View(userRewards);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userReward = await _context.UserRewards
            .Include(ur => ur.Reward)
                .ThenInclude(r => r.Project)
            .FirstOrDefaultAsync(ur => ur.UserRewardId == id && ur.UserId == userId);

        if (userReward == null)
        {
            return NotFound();
        }

        return View(userReward);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignReward(string userId, int rewardId)
    {
        try
        {
            var userReward = new UserReward
            {
                UserId = userId,
                RewardId = rewardId,
                DateAwarded = DateTime.Now
            };

            // Check if user already has this reward
            var existingReward = await _context.UserRewards
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RewardId == rewardId);

            if (existingReward != null)
            {
                return BadRequest("User already has this reward");
            }

            _context.UserRewards.Add(userReward);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reward {rewardId} assigned to user {userId}");
            return Ok("Reward assigned successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning reward: {ex.Message}");
            return StatusCode(500, "Error assigning reward");
        }
    }

    [Authorize]
    public async Task<IActionResult> CheckEligibility(int projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get user's total contribution to this project
        var totalContribution = await _context.Contributions
            .Where(c => c.UserId == userId && c.ProjectId == projectId)
            .SumAsync(c => c.Amount);

        // Get all rewards for this project
        var eligibleRewards = await _context.Rewards
            .Where(r => r.ProjectId == projectId && r.MinimumContribution <= totalContribution)
            .ToListAsync();

        // Get rewards user already has
        var existingRewards = await _context.UserRewards
            .Where(ur => ur.UserId == userId && ur.Reward.ProjectId == projectId)
            .Select(ur => ur.RewardId)
            .ToListAsync();

        // Filter out rewards user already has
        var newEligibleRewards = eligibleRewards
            .Where(r => !existingRewards.Contains(r.RewardId))
            .ToList();

        // Automatically assign new eligible rewards
        foreach (var reward in newEligibleRewards)
        {
            var userReward = new UserReward
            {
                UserId = userId,
                RewardId = reward.RewardId,
                DateAwarded = DateTime.Now
            };
            _context.UserRewards.Add(userReward);
        }

        if (newEligibleRewards.Any())
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"New rewards assigned to user {userId} for project {projectId}");
        }

        return Json(new
        {
            totalContribution,
            newRewardsCount = newEligibleRewards.Count,
            allEligibleRewards = eligibleRewards
        });
    }

    [Authorize]
    public async Task<IActionResult> GetProjectRewards(int projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userRewards = await _context.UserRewards
            .Include(ur => ur.Reward)
            .Where(ur => ur.UserId == userId && ur.Reward.ProjectId == projectId)
            .OrderBy(ur => ur.Reward.MinimumContribution)
            .ToListAsync();

        return Json(userRewards);
    }
}