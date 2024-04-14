namespace Gbc_Travel_Group63.Models{
    public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public string BookingType { get; set; }

    public bool IsGuest { get; set; }

    public int ItemId { get; set; }
    // Add other necessary properties

    // Navigation property if needed
    // public User User { get; set; }
}
}