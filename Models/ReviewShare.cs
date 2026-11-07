using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class ReviewShare
    {
        public int Id { get; set; }

        [Required]
        public int ReviewDraftId { get; set; }
        public ReviewDraft? ReviewDraft { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Display(Name = "Shared At")]
        public DateTime SharedAt { get; set; } = DateTime.Now;
    }
}