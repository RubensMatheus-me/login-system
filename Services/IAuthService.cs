using login_system.Models.Response;
using LoginRequest = login_system.Models.Request.LoginRequest;
using RegisterRequest = login_system.Models.Request.RegisterRequest;

namespace login_system.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(LoginRequest request);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task SendEmail(string email, string verificationToken);
    Task ResetPasswordAsync(string email, string code);
    Task SendRecoveryPasswordAsync(string email);
}