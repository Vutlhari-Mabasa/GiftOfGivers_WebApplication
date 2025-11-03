using System;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class ResourceTracking
    {
        [Key]
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Resource name is required")]
        [Display(Name = "Resource Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; } = string.Empty;

        [Display(Name = "Category")]
        public string Category { get; set; } = "General";

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Donated By")]
        public string? DonatedBy { get; set; }

        [Display(Name = "Donation Date")]
        [DataType(DataType.DateTime)]
        public DateTime DonationDate { get; set; } = DateTime.Now;

        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium";

        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Display(Name = "Expiry Date (if applicable)")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
    }
}
