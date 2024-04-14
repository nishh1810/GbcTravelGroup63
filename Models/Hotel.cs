using System.ComponentModel.DataAnnotations;

namespace Gbc_Travel_Group63.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Hotel name is required")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5")]
        public int StarRating { get; set; }

        public decimal PricePerNight { get; set; }

        public bool IsPetFriendly { get; set; }

        [Required(ErrorMessage = "Room type is required")]
        public string RoomType { get; set; }
    }
}
