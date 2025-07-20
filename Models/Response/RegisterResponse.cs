namespace login_system.Models.Response;

public class RegisterResponse
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = "Usuário registrado com sucesso.";
}