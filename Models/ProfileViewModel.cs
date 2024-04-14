using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Gbc_Travel_Group63.Models
{
    public class ProfileViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int profileNumber { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Preferences { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}