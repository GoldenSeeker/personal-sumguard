using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // list all drafts
        public async Task<IActionResult> Index()
        {
            var drafts = await _context.ReviewDrafts.ToListAsync();
            return View(drafts);
        }

        // show the empty form
        public IActionResult CreateDraft()
        {
            return View();
        }

        // save the submitted form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDraft(ReviewDraft draft)
        {
            if (ModelState.IsValid)
            {
                _context.ReviewDrafts.Add(draft);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(draft);
        }
                // GET: /Review/AttachRecord/5 — show the attach form for draft #5
        public async Task<IActionResult> AttachRecord(int id)
        {
            var draft = await _context.ReviewDrafts.FindAsync(id);
            if (draft == null)
            {
                return NotFound();
            }

            ViewBag.RecallRecords = await _context.RecallRecords.ToListAsync();
            return View(draft);
        }

        // POST: /Review/AttachRecord/5 — save the chosen recall record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachRecord(int id, int recallRecordId)
        {
            var draft = await _context.ReviewDrafts.FindAsync(id);
            if (draft == null)
            {
                return NotFound();
            }

            draft.RecallRecordId = recallRecordId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
                // GET: /Review/ShareReview/5 — show sharing form for draft #5
        public async Task<IActionResult> ShareReview(int id)
        {
            var draft = await _context.ReviewDrafts
                .Include(d => d.Shares)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (draft == null) return NotFound();

            return View(draft);
        }

        // POST: /Review/ShareReview/5 — save a new share entry and mark the draft as shared
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShareReview(int id, string customerEmail)
        {
            var draft = await _context.ReviewDrafts.FindAsync(id);
            if (draft == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                _context.ReviewShares.Add(new ReviewShare
                {
                    ReviewDraftId = id,
                    CustomerEmail = customerEmail.Trim(),
                    SharedAt = DateTime.Now
                });
                draft.IsShared = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ShareReview), new { id });
        }

        // download a plain-text summary
        public async Task<IActionResult> ExportSummary(int id)
        {
            var draft = await _context.ReviewDrafts
                .Include(d => d.RecallRecord)
                .Include(d => d.Shares)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (draft == null) return NotFound();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("REVIEW SUMMARY");
            sb.AppendLine("==============");
            sb.AppendLine($"Title:        {draft.Title}");
            sb.AppendLine($"Created At:   {draft.CreatedAt:yyyy-MM-dd HH:mm}");
            sb.AppendLine($"Shared:       {(draft.IsShared ? "Yes" : "No")}");
            sb.AppendLine($"Recall:       {draft.RecallRecord?.RecallNumber ?? "Not attached"}");
            sb.AppendLine($"Product:      {draft.RecallRecord?.ProductName ?? "—"}");
            sb.AppendLine();
            sb.AppendLine("Content:");
            sb.AppendLine(draft.Content);
            sb.AppendLine();
            sb.AppendLine("Shared With:");
            if (draft.Shares.Any())
            {
                foreach (var share in draft.Shares)
                {
                    sb.AppendLine($" - {share.CustomerEmail} (shared {share.SharedAt:yyyy-MM-dd HH:mm})");
                }
            }
            else
            {
                sb.AppendLine(" (no one yet)");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", $"review-{draft.Id}-summary.txt");
        }
    }
}