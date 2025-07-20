using System.ComponentModel.DataAnnotations;

namespace login_system.Models.Request;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmedPassword { get; set; } = string.Empty;
}