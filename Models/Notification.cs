using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class Notification
    {
        public int Id { get; set; }

        // Who receives this notification (internal staff email, or "admin" for now)
        [Required]
        [StringLength(200)]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; } = string.Empty;

        // Short message text
        [Required]
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        // Optional: link to the item that triggered the notification
        [StringLength(300)]
        [Display(Name = "Link")]
        public string? Link { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Read")]
        public bool IsRead { get; set; } = false;
    }
}