using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    public class CustomerPortalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerPortalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /CustomerPortal?email=test@example.com
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return View("EnterEmail");
            }

            var sharedDrafts = await _context.ReviewDrafts
                .Include(d => d.RecallRecord)
                .Include(d => d.Comments)
                .Where(d => d.Shares.Any(s => s.CustomerEmail == email))
                .ToListAsync();

            ViewBag.Email = email;
            return View(sharedDrafts);
        }

        // GET: /CustomerPortal/Details/5?email=test@example.com
        public async Task<IActionResult> Details(int id, string email)
        {
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
        public async Task<IActionResult> AddComment(int id, string email, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return RedirectToAction(nameof(Details), new { id, email });
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
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id, email });
        }

        // GET: /CustomerPortal/ExportDocumentation/5?email=test@example.com
        public async Task<IActionResult> ExportDocumentation(int id, string email)
        {
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