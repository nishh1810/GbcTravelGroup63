using System.ComponentModel.DataAnnotations;

namespace Gbc_Travel_Group63.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The CarBrand field is required.")]
        public string CarBrand { get; set; }

        public string CarModel { get; set; }

        [Range(1900, 2100, ErrorMessage = "The field Year must be between {1} and {2}.")]
        public int Year { get; set; }

        public string Color { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
