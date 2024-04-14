using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gbc_Travel_Group63.Models
{
    public class Flights
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightNumber { get; set; }

        [Required(ErrorMessage = "Departure city is required.")]
        public string DepartureCity { get; set; }

        [Required(ErrorMessage = "Arrival city is required.")]
        public string ArrivalCity { get; set; }

      
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

       
        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }

        
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number of passengers must be greater than 0.")]
        public int? NumberOfPassengers { get; set; } // Nullable integer

        [Range(1, int.MaxValue, ErrorMessage = "Total seats must be greater than 0.")]
        public int TotalSeats { get; } = 450;

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; } // Add the Price property
    }
}
