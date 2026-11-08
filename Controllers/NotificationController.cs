using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Data;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        // For now, all notifications go to a fixed "staff" recipient.
        // In Group 4 we'll tie this to logged-in users.
        private const string StaffRecipient = "staff";

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Notification
        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Notifications
                .Where(n => n.Recipient == StaffRecipient)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notifications);
        }

        // POST: /Notification/MarkRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Notification/MarkAllRead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllRead()
        {
            var unread = await _context.Notifications
                .Where(n => n.Recipient == StaffRecipient && !n.IsRead)
                .ToListAsync();
            foreach (var n in unread)
            {
                n.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper: called from other controllers to create a notification
        public static void Notify(ApplicationDbContext context, string message, string? link = null)
        {
            context.Notifications.Add(new Notification
            {
                Recipient = StaffRecipient,
                Message = message,
                Link = link
            });
            // Caller is responsible for SaveChangesAsync
        }
    }
}