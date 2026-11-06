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
    }
}