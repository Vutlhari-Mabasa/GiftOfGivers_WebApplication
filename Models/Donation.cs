using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfGivers_WebApplication.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [ForeignKey("Donor")]
        public int DonorID { get; set; }

        [ForeignKey("ReliefProject")]
        public int ReliefProjectID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Donor Donor { get; set; }
        public ReliefProject ReliefProject { get; set; }
    }
}
