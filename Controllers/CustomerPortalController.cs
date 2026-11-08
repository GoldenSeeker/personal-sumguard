using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    [Authorize]
    public class CustomerPortalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerPortalController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Helper: the email of the currently logged-in user
        private async Task<string?> GetCurrentEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Email;
        }

        // GET: /CustomerPortal
        public async Task<IActionResult> Index()
        {
            var email = await GetCurrentEmailAsync();
            if (string.IsNullOrEmpty(email))
            {
                return Forbid();
            }

            var sharedDrafts = await _context.ReviewDrafts
                .Include(d => d.RecallRecord)
                .Include(d => d.Comments)
                .Where(d => d.Shares.Any(s => s.CustomerEmail == email))
                .ToListAsync();

            ViewBag.Email = email;
            return View(sharedDrafts);
        }

        // GET: /CustomerPortal/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var email = await GetCurrentEmailAsync();
            if (string.IsNullOrEmpty(email)) return Forbid();

            var draft = await _context.ReviewDrafts
                .Include(d => d.RecallRecord)
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (draft == null) return NotFound();

            var isShared = await _context.ReviewShares
                .AnyAsync(s => s.ReviewDraftId == id && s.CustomerEmail == email);
            if (!isShared) return Forbid();

            ViewBag.Email = email;
            return View(draft);
        }

        // POST: /CustomerPortal/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, string text)
        {
            var email = await GetCurrentEmailAsync();
            if (string.IsNullOrEmpty(email)) return Forbid();

            if (string.IsNullOrWhiteSpace(text))
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            var isShared = await _context.ReviewShares
                .AnyAsync(s => s.ReviewDraftId == id && s.CustomerEmail == email);
            if (!isShared) return Forbid();

            _context.CustomerComments.Add(new CustomerComment
            {
                ReviewDraftId = id,
                CustomerEmail = email,
                Text = text.Trim(),
                PostedAt = DateTime.Now
            });

            NotificationController.Notify(
                _context,
                $"New comment from {email} on review #{id}",
                Url.Action("Details", "Review", new { id }));

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /CustomerPortal/ExportDocumentation/5
        public async Task<IActionResult> ExportDocumentation(int id)
        {
            var email = await GetCurrentEmailAsync();
            if (string.IsNullOrEmpty(email)) return Forbid();

            var draft = await _context.ReviewDrafts
                .Include(d => d.RecallRecord)
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (draft == null) return NotFound();

            var isShared = await _context.ReviewShares
                .AnyAsync(s => s.ReviewDraftId == id && s.CustomerEmail == email);
            if (!isShared) return Forbid();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("FINAL REVIEW DOCUMENTATION");
            sb.AppendLine("===========================");
            sb.AppendLine($"Customer:     {email}");
            sb.AppendLine($"Title:        {draft.Title}");
            sb.AppendLine($"Created At:   {draft.CreatedAt:yyyy-MM-dd HH:mm}");
            sb.AppendLine($"Recall:       {draft.RecallRecord?.RecallNumber ?? "Not attached"}");
            sb.AppendLine($"Product:      {draft.RecallRecord?.ProductName ?? "—"}");
            sb.AppendLine();
            sb.AppendLine("Review Content:");
            sb.AppendLine(draft.Content);
            sb.AppendLine();
            sb.AppendLine("Customer Comments:");
            if (draft.Comments.Any())
            {
                foreach (var comment in draft.Comments.OrderBy(c => c.PostedAt))
                {
                    sb.AppendLine($" - [{comment.PostedAt:yyyy-MM-dd HH:mm}] {comment.CustomerEmail}: {comment.Text}");
                }
            }
            else
            {
                sb.AppendLine(" (no comments yet)");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", $"final-doc-{draft.Id}.txt");
        }
    }
}