﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingApp.Models;
using CrowdFundingApp.ViewModels;

namespace CrowdFundingApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly CrowdFundingDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(CrowdFundingDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                return RedirectToAction("UserDashboard");
            }
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            var users = _context.Users.ToList();
            var projects = _context.Projects.ToList();
            var contributions = _context.Contributions.ToList();
            var rewards = _context.Rewards.ToList();
            var userRewards = _context.UserRewards
                .Include(ur => ur.User)
                .Include(ur => ur.Reward)
                .ToList();

            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                Projects = projects,
                UserRewards = userRewards,
                Rewards = rewards,
                Contributions = contributions
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserDashboard()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

 
            var userProjects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();


 
            var userContributions = await _context.Contributions
                .Where(c => c.UserId == userId)
                .Include(c => c.Project)
                .ToListAsync();


 
            var userRewards = await _context.UserRewards
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Reward)
                .ToListAsync();


 
            var projectCategory = await _context.Categories.ToListAsync();



            var viewModel = new UserDashboardViewModel
            {
                Projects = userProjects,
                Contributions = userContributions,
                UserRewards = userRewards,
                Categories = projectCategory
            };

            return View(viewModel);
        }
    }
}
