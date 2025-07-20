using login_system.Models;
using login_system.Models.Request;
using login_system.Models.Response;
using login_system.Repositories;
using login_system.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ForgotPasswordRequest = login_system.Models.Request.ForgotPasswordRequest;
using LoginRequest = login_system.Models.Request.LoginRequest;
using RegisterRequest = login_system.Models.Request.RegisterRequest;
using ResetPasswordRequest = login_system.Models.Request.ResetPasswordRequest;

namespace login_system.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    

    public AuthController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.AuthenticateAsync(request);
            return Ok(new LoginResponse { Token = token! });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _authService.RegisterAsync(request);
            return Created("", response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpGet("verify")]
    public async Task<IActionResult> Verify([FromQuery] string token)
    {
        try
        {
            var user = await _userRepository.GetByVerificationToken(token);
            if (user == null)
                return BadRequest(new { error = "Token inválido." });

            user.Active = true;
            user.VerificationToken = null;
            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "Conta ativada com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno ao verificar conta.", details = ex.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new { error = "Email é obrigatório." });

            await _authService.SendRecoveryPasswordAsync(request.Email);
            return Ok(new { message = "Um link de recuperação será enviado." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await _authService.ResetPasswordAsync(request.Code, request.NewPassword);
            return Ok(new { message = "Senha redefinida com sucesso." });
            
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
