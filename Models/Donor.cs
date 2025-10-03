using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔗 One Donor → Many Donations
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}
