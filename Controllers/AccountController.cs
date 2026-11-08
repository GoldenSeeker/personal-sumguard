using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Register?token=abc123
        public async Task<IActionResult> Register(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Content("Missing invitation token.");
            }

            var invite = await _context.InvitationTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (invite == null) return Content("Invalid invitation link.");
            if (invite.IsUsed) return Content("This invitation has already been used.");
            if (invite.ExpiresAt < DateTime.Now) return Content("This invitation has expired.");

            ViewBag.Email = invite.Email;
            ViewBag.Token = invite.Token;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string token, string displayName, string password)
        {
            var invite = await _context.InvitationTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (invite == null || invite.IsUsed || invite.ExpiresAt < DateTime.Now)
            {
                return Content("Invalid or expired invitation.");
            }

            if (string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Display name and password are required.");
                ViewBag.Email = invite.Email;
                ViewBag.Token = invite.Token;
                return View();
            }

            // Check if a user with this email already exists
            var existing = await _userManager.FindByEmailAsync(invite.Email);
            if (existing != null)
            {
                return Content("An account with this email already exists.");
            }

            // Create the Identity user
            var user = new ApplicationUser
            {
                UserName = invite.Email,
                Email = invite.Email,
                DisplayName = displayName.Trim(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return Content($"Registration failed: {errors}");
            }

            // Mark the invitation as used
            invite.IsUsed = true;
            await _context.SaveChangesAsync();

            return Content($"Registered! Welcome, {displayName}. You can now <a href='/Account/Login'>log in</a>.");
        }
                // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName: email,
                password: password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Review");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}