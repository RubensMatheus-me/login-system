using System.Security.Cryptography;
using login_system.Data;
using login_system.Models;
using login_system.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace login_system.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IRecoveryPasswordRepository _recoveryPasswordRepository;

    public UserRepository(AppDbContext context, IEmailService emailService,
        IRecoveryPasswordRepository recoveryPasswordRepository, IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _recoveryPasswordRepository = recoveryPasswordRepository;
        _configuration = configuration;
    }

    public async Task<User?> GetEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Active);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByVerificationToken(string token)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailIgnoreActiveAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User?>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task DeleteUserAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}