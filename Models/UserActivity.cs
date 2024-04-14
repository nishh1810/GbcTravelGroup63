using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gbc_Travel_Group63.Models
{
    public class UserActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? departureCity { get; set; }
        public string? arrivalCity { get; set; }
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }
        public string? location { get; set; }
        public decimal? pricePerDay { get; set; }
        public string? roomType {  get; set; }
        public string? pickupLocation { get; set; }
        public string? brand { get; set; }
        public string status { get; } = "Active";
    }
}
