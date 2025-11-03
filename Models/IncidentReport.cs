using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class IncidentReport
    {
        [Key]
        public int IncidentID { get; set; }

        [Required(ErrorMessage = "Incident name is required")]
        [Display(Name = "Incident Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Incident type is required")]
        [Display(Name = "Type of Disaster")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location/Area")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Incident Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Severity Level")]
        public string SeverityLevel { get; set; } = "Medium";

        [Display(Name = "Estimated Affected People")]
        public int? EstimatedAffectedPeople { get; set; }

        [Display(Name = "Current Status")]
        public string Status { get; set; } = "Active";

        [Display(Name = "Reported By")]
        public string? ReportedBy { get; set; }

        [Display(Name = "Contact Information")]
        public string? ContactInformation { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔗 One Incident → Many Relief Projects
        public ICollection<ReliefProject> ReliefProjects { get; set; } = new List<ReliefProject>();
    }
}