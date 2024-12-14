using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CrowdFundingApp.ViewModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CrowdFundingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                _logger.LogInformation("User logged in.");
                //return LocalRedirect(returnUrl);
                return RedirectToAction("Index", "Home");
                //if (result.Succeeded)
                //{

                //    //return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return View(model);
                //}
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
#pragma warning disable CS8604 // Possible null reference argument.
                var result = await _userManager.CreateAsync(user, model.Password);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "User");
                    return LocalRedirect(returnUrl);
                    //return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            _logger.LogInformation("Login POST method called for {Email}", model.Email);
            _logger.LogWarning("Login failed for {Email}", model.Email);
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}