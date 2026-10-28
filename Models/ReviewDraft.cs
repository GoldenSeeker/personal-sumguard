using System.ComponentModel.DataAnnotations; // Needed for [Required] and other validation attributes

namespace Personal_Sumguard.Models 
{
    // This class represents a single "review draft"
    public class ReviewDraft
    {
        // Every record needs a unique ID, like an identity number
        public int Id { get; set; }

        // [Required] means this field cannot be empty
        // {0} is replaced by the field name, so the error reads "Title is required"
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200)] // Max 200 characters to prevent overly long input
        [Display(Name = "Title")] // Shown as "Title" on the page
        public string Title { get; set; } = string.Empty;

        // The body of the draft, also required
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;

        // Records when this draft was created; defaults to the current time
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Marks whether this draft has been shared with the customer; defaults to false
        [Display(Name = "Is Shared")]
        public bool IsShared { get; set; } = false;
    }
}