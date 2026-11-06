using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class RecallRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50)]
        [Display(Name = "Recall Number")]
        public string RecallNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Recall Date")]
        [DataType(DataType.Date)]
        public DateTime RecallDate { get; set; } = DateTime.Today;

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}