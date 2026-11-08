using Microsoft.AspNetCore.Identity;

namespace Personal_Sumguard.Models
{
    // Extends the built-in IdentityUser with any custom fields you need later.
    // For now, it's identical to IdentityUser, but having a custom class
    // makes future additions (like DisplayName) easier.
    public class ApplicationUser : IdentityUser
    {
        // You can add custom properties here later, e.g.:
        // public string? DisplayName { get; set; }
    }
}