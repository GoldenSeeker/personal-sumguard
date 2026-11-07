using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class CustomerComment
    {
        public int Id { get; set; }

        [Required]
        public int ReviewDraftId { get; set; }
        public ReviewDraft? ReviewDraft { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(2000)]
        [Display(Name = "Comment")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Posted At")]
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }
}