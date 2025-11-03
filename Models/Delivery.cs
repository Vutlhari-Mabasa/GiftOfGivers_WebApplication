using System;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DeliveredAt { get; set; }
    }
}
