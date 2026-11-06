using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class ReviewDraft
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Is Shared")]
        public bool IsShared { get; set; } = false;

        [Display(Name = "Attached Recall")]
        public int? RecallRecordId { get; set; }

        public RecallRecord? RecallRecord { get; set; }
    }
}