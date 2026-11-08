using System.ComponentModel.DataAnnotations;

namespace Personal_Sumguard.Models
{
    public class SearchFilterModel
    {
        // Free-text keyword matched against RecallNumber OR ProductName
        [Display(Name = "Keyword")]
        public string? Keyword { get; set; }

        // Only records whose product name contains this text
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        // Date range (both optional)
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        // The results (populated by the controller after querying)
        public List<RecallRecord> Results { get; set; } = new();

        // True once the user has submitted the form at least once
        public bool HasSearched { get; set; }
    }
}