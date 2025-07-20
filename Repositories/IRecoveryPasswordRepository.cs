using login_system.Models;

namespace login_system.Repositories;

public interface IRecoveryPasswordRepository
{
    Task AddAsync(RecoveryPassword recoveryPassword);
    Task<RecoveryPassword> GetByCodeAsync(string code);
    Task DeleteAsync(RecoveryPassword recoveryPassword);
}