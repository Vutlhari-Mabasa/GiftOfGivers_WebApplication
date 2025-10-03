using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class IncidentReport
    {
        [Key]
        public int IncidentID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        // 🔗 One Incident → Many Relief Projects
        public ICollection<ReliefProject> ReliefProjects { get; set; } = new List<ReliefProject>();
    }
}