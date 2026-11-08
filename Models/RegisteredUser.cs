using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class RegisteredUser
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        // In Group 4 this becomes a proper hashed password.
        // For now we store it plainly with a big warning: DO NOT DO THIS IN PRODUCTION.
        [Required]
        [StringLength(100)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = string.Empty;

        [Display(Name = "Registered At")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}