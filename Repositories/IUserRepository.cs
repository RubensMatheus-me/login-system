using login_system.Models;

namespace login_system.Repositories;

public interface IUserRepository
{
    Task<User?> GetEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(User user);
    Task<User?> GetByVerificationToken(string token);
    Task UpdateAsync(User user);
    Task<User?> GetUserByEmailIgnoreActiveAsync(string email);
    Task<IEnumerable<User?>> GetAllAsync();
    Task<User?> GetByIdAsync(long id);
    Task DeleteUserAsync(long id);
}