using System.Reflection.Metadata;

namespace login_system.Models.Request;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}