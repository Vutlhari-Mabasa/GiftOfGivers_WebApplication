using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Status { get; set; }
    }
}
