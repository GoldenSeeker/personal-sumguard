using Microsoft.AspNetCore.Identity;

namespace Personal_Sumguard.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Custom field for the friendly name shown in the UI
        public string? DisplayName { get; set; }
    }
}