using System.ComponentModel.DataAnnotations;

namespace Presentation.BlazorApp.Models.Authentication;

public class SignInModel
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email address is required")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
