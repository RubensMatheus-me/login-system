namespace login_system.Models;

public class User
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string? VerificationToken { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<RecoveryPassword>? RecoveryRequests { get; set; }
}