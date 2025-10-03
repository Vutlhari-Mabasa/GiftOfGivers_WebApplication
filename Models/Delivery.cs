using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfGivers_WebApplication.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        [ForeignKey("ReliefProject")]
        public int ReliefProjectID { get; set; }

        [ForeignKey("ResourceTracking")]
        public int ResourceID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DeliveredAt { get; set; }

        public ReliefProject ReliefProject { get; set; }
        public ResourceTracking ResourceTracking { get; set; }
    }
}
