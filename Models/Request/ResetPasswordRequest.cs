namespace login_system.Models.Request;

public class ResetPasswordRequest
{
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

}