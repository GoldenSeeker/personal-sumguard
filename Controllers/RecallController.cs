using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    [Authorize]
    public class RecallController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecallController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Recall/Search?Keyword=...&ProductName=...&FromDate=...&ToDate=...
        public async Task<IActionResult> Search(SearchFilterModel filter)
        {
            // Build the query step by step
            IQueryable<RecallRecord> query = _context.RecallRecords;

            // Only apply each filter if the user actually supplied a value
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim().ToLower();
                query = query.Where(r =>
                    r.RecallNumber.ToLower().Contains(kw) ||
                    r.ProductName.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                var pn = filter.ProductName.Trim().ToLower();
                query = query.Where(r => r.ProductName.ToLower().Contains(pn));
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(r => r.RecallDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(r => r.RecallDate <= filter.ToDate.Value);
            }

            // Did the user submit anything?
            filter.HasSearched =
                !string.IsNullOrWhiteSpace(filter.Keyword) ||
                !string.IsNullOrWhiteSpace(filter.ProductName) ||
                filter.FromDate.HasValue ||
                filter.ToDate.HasValue;

            // Only hit the DB and return results if a search was actually made
            if (filter.HasSearched)
            {
                filter.Results = await query
                    .OrderByDescending(r => r.RecallDate)
                    .ToListAsync();
            }

            return View(filter);
        }
    }
}