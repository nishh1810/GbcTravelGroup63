using Microsoft.AspNetCore.Identity;

namespace Gbc_Travel_Group63.Models
{
    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties here if needed
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Preferences { get; set; }

        // Property for profile picture
        public byte[]? ProfilePicture { get; set; }
    }
}
