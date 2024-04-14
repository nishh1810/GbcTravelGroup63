using System.ComponentModel.DataAnnotations;
namespace Gbc_Travel_Group63.Models{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
