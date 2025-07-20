using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using login_system.Models;
using login_system.Models.Response;
using login_system.Repositories;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = login_system.Models.Request.LoginRequest;
using RegisterRequest = login_system.Models.Request.RegisterRequest;

namespace login_system.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IRecoveryPasswordRepository _recoveryPasswordRepository;

    public AuthService(IConfiguration configuration, IUserRepository userRepository, IEmailService emailService, IRecoveryPasswordRepository recoveryPasswordRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _emailService = emailService;
        _recoveryPasswordRepository = recoveryPasswordRepository;
    }

    public async Task<string?> AuthenticateAsync(LoginRequest request)
    {
        var user = await _userRepository.GetUserByEmailIgnoreActiveAsync(request.Email);

        if (user == null ) 
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
            
        if (!user.Active) 
            throw new UnauthorizedAccessException("Conta não ativada. Verifique seu email para ativar a conta.");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Email ou senha inválidos.");

        return GenerateJwtToken(user);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var verificationToken = Guid.NewGuid().ToString();
        
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new ArgumentException("O e-mail já está em uso");

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Active = false,
            VerificationToken = verificationToken
        };
        
        var baseVerificationLink = _configuration["Email:VerificationLink"];
        var verificationLink = baseVerificationLink + verificationToken;
        await SendEmail(user.Email, verificationLink);

        await _userRepository.AddAsync(user);
        
        return new RegisterResponse
        {
            Id = user.Id,
            Email = user.Email
        };
    }

    public async Task SendEmail(string email, string verificationLink)
    {
        var templatePath = "Templates/Emails/VerificationEmailTemplate.html";
        var replacements = new Dictionary<string, string>
        {
            { "{{VERIFICATION_LINK}}", verificationLink }
        };

        var htmlBody = _emailService.LoadTemplateAndReplace(templatePath, replacements);
        await _emailService.SendEmailAsync(email, "Verificação de conta", htmlBody);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task ResetPasswordAsync(string code, string newPassword)
    {
        var recovery = await _recoveryPasswordRepository.GetByCodeAsync(code);

        if (recovery == null)
        {
            throw new Exception("Código de recuperação inválido.");
        }

        if (recovery.TimeExpiration < DateTime.UtcNow)
        {
            throw new Exception("Código expirado.");
        }

        if (recovery.User == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var user = recovery.User;
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        await  _userRepository.UpdateAsync(user);
        
        await _recoveryPasswordRepository.DeleteAsync(recovery);
    }

    public async Task SendRecoveryPasswordAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailIgnoreActiveAsync(email);
        if (user == null) return;

        if (user.Email == null)
        {
            throw new Exception("Usuário não possui email.");
        }

        var code = GenerateRandomNumber(6);

        var recoveryPassword = new RecoveryPassword
        {
            UserId = user.Id,
            Code = code,
            TimeExpiration = DateTime.UtcNow.AddMinutes(30)
        };
        
        await _recoveryPasswordRepository.AddAsync(recoveryPassword);

        var recoveryLink = _configuration["Email:RecoveryLink"] + code;

        var templatePath = "Templates/Emails/RecoveryPasswordTemplate.html";
        
        var replacements = new Dictionary<string, string>
        {
            { "{{RECOVERY_LINK}}", recoveryLink }
        };

        var htmlBody = _emailService.LoadTemplateAndReplace(templatePath, replacements);
        await _emailService.SendEmailAsync(email, "Recuperação de senha", htmlBody);
    }

    private string GenerateRandomNumber(int lenght)
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[lenght];
        rng.GetBytes(bytes);

        var digits = new char[lenght];
        for (int i = 0; i < lenght; i++)
        {
            digits[i] = (char)('0' + (bytes[i] % 10));
        }

        return new string(digits);
    }
}