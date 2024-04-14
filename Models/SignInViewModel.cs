using System.ComponentModel.DataAnnotations;

namespace Gbc_Travel_Group63.Models
{
public class SignInViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe {get; set;}
}
}