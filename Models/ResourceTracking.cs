using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfGivers_WebApplication.Models
{
    public class ResourceTracking
    {
        [Key]
        public int ResourceID { get; set; }

        [ForeignKey("ReliefProject")]
        public int ReliefProjectID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Unit { get; set; }

        public ReliefProject ReliefProject { get; set; }

        // 🔗 One Resource → Many Deliveries
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
