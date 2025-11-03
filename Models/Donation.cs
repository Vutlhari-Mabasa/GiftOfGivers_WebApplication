using System;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Type { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
