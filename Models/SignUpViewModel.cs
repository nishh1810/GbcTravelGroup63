using System.ComponentModel.DataAnnotations;

namespace Gbc_Travel_Group63.Models{
    public class SignupViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public bool IsAdmin { get; set; }
    }
}