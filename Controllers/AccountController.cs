using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
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

            if (invite == null)
            {
                return Content("Invalid invitation link.");
            }
            if (invite.IsUsed)
            {
                return Content("This invitation has already been used.");
            }
            if (invite.ExpiresAt < DateTime.Now)
            {
                return Content("This invitation has expired.");
            }

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

            // Check for an existing account with this email
            var existing = await _context.RegisteredUsers
                .AnyAsync(u => u.Email == invite.Email);
            if (existing)
            {
                return Content("An account with this email already exists.");
            }

            _context.RegisteredUsers.Add(new RegisteredUser
            {
                Email = invite.Email,
                Password = password,   // ⚠️ plain text for now; hashed in Group 4
                DisplayName = displayName.Trim(),
                RegisteredAt = DateTime.Now
            });

            invite.IsUsed = true;
            await _context.SaveChangesAsync();

            return Content($"Registered! Welcome, {displayName}. You can now log in (login form comes in Group 4).");
        }
    }
}