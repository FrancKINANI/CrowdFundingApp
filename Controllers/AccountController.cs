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
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Email or password incorrect.");
                    return View(model);
                }

                if (!user.EmailConfirmed)
                {
                    // Instead of showing the message with a URL, we'll pass a flag to the view
                    ViewBag.NeedsVerification = true;
                    ViewBag.UserEmail = model.Email;
                    ViewBag.VerificationUrl = Url.Action("VerifyAccount", "Account", new { email = model.Email });
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {model.Email} logged in successfully");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning($"User {model.Email} account locked out");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email or password incorrect.");
                    return View(model);
                }
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
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Bio = model.Bio,
                    CreatedAt = DateTime.Now
                };

                if (model.UserType == "Startup")
                {
                    user.CompanyName = model.CompanyName;
                    user.BusinessDescription = model.BusinessDescription;
                }
                else if (model.UserType == "Investor")
                {
                    user.InvestmentCapacity = model.InvestmentCapacity;
                    user.InvestmentPreferences = model.InvestmentPreferences;
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    try 
                    {
                        await _userManager.AddToRoleAsync(user, model.UserType);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation($"User created a new account as {model.UserType}");
                        return RedirectToLocal(returnUrl);
                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError(string.Empty, "Error creating account. Please try again.");
                        _logger.LogError($"Error creating user: {ex.Message}");
                        return View(model);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyAccount(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmVerification(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Login", new { verified = true });
            }
            return RedirectToAction("Register");
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

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }
            return View();
        }
    }
}