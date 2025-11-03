using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfGivers_WebApplication.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectID { get; set; }

        [ForeignKey("IncidentReport")]
        public int? IncidentID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Status { get; set; }

        public IncidentReport IncidentReport { get; set; }

        // 🔗 One Project → Many Donations
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();

        // 🔗 One Project → Many Resources
        public ICollection<ResourceTracking> Resources { get; set; } = new List<ResourceTracking>();

        // 🔗 One Project → Many Deliveries
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

        // 🔗 One Project → Many Volunteer Tasks
        public ICollection<VolunteerTask> VolunteerTasks { get; set; } = new List<VolunteerTask>();
    }
}
