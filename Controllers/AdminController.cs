using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin/DataManagement
        public async Task<IActionResult> DataManagement()
        {
            ViewBag.TotalDrafts = await _context.ReviewDrafts.CountAsync();
            ViewBag.UnsharedDrafts = await _context.ReviewDrafts.CountAsync(d => !d.IsShared);
            ViewBag.TotalComments = await _context.CustomerComments.CountAsync();
            ViewBag.UnreadNotifications = await _context.Notifications.CountAsync(n => !n.IsRead);
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalRoles = await _roleManager.Roles.CountAsync();

            return View();
        }

        // GET: /Admin/Roles — list roles and members (Story 13 seed)
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new Dictionary<string, List<string>>();

            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name!);
                viewModel[role.Name!] = users.Select(u => u.Email ?? "(no email)").ToList();
            }

            return View(viewModel);
        }

        // POST: /Admin/RunCleanup — manual trigger for the background job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunCleanup()
        {
            var cutoff = DateTime.Now.AddDays(-30);
            var cleaned = 0;

            // Delete notifications older than 30 days that are already read
            var oldRead = await _context.Notifications
                .Where(n => n.IsRead && n.CreatedAt < cutoff)
                .ToListAsync();
            _context.Notifications.RemoveRange(oldRead);
            cleaned += oldRead.Count;

            await _context.SaveChangesAsync();

            TempData["CleanupResult"] = $"Cleaned up {cleaned} old notification(s).";
            return RedirectToAction(nameof(DataManagement));
        }

        // POST: /Admin/AssignRole — add a user to a role by email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["RoleResult"] = $"No user with email {email}.";
                return RedirectToAction(nameof(Roles));
            }
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["RoleResult"] = $"Role '{roleName}' does not exist.";
                return RedirectToAction(nameof(Roles));
            }
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
                TempData["RoleResult"] = $"{email} added to {roleName}.";
            }
            else
            {
                TempData["RoleResult"] = $"{email} is already in {roleName}.";
            }
            return RedirectToAction(nameof(Roles));
        }

        // POST: /Admin/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName) && !await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                TempData["RoleResult"] = $"Role '{roleName}' created.";
            }
            return RedirectToAction(nameof(Roles));
        }
    }
}