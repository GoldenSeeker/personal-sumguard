using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class InvitationToken
    {
        public int Id { get; set; }

        // The email this invitation was sent to
        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        // The unique random token that appears in the invite link
        [Required]
        [StringLength(100)]
        public string Token { get; set; } = string.Empty;

        // Whether the invitation has already been used
        [Display(Name = "Used")]
        public bool IsUsed { get; set; } = false;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Expires At")]
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(7);
    }
}