using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class RecallRecord
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Recall Number")]
        public string RecallNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Product Description")]
        public string? ProductDescription { get; set; }

        [StringLength(200)]
        [Display(Name = "Trade Name")]
        public string? TradeName { get; set; }

        [StringLength(100)]
        [Display(Name = "Recall Class")]
        public string? RecallClass { get; set; }

        [StringLength(200)]
        [Display(Name = "Firm Name")]
        public string? FirmName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Recall Reason")]
        public string? RecallReason { get; set; }

        [Display(Name = "Classification Date")]
        [DataType(DataType.Date)]
        public DateTime? ClassificationDate { get; set; }

        [Display(Name = "Posted Date")]
        [DataType(DataType.Date)]
        public DateTime? PostedDate { get; set; }

        [Display(Name = "Termination Date")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        // Kept so existing code (Story 2 attach flow) still works
        [Display(Name = "Recall Date")]
        [DataType(DataType.Date)]
        public DateTime RecallDate { get; set; } = DateTime.Today;

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}