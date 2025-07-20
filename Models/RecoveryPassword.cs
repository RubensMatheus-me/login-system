namespace login_system.Models;

public class RecoveryPassword
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime TimeExpiration { get; set; }
    
    public User? User { get; set; }

}