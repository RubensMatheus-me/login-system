using login_system.Data;
using login_system.Models;
using Microsoft.EntityFrameworkCore;

namespace login_system.Repositories;

public class RecoveryPasswordRepository :IRecoveryPasswordRepository
{
    private readonly AppDbContext _context;

    public RecoveryPasswordRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(RecoveryPassword recoveryPassword)
    {
        _context.RecoveryPassword.Add(recoveryPassword);
        await _context.SaveChangesAsync();
    }

    public async Task<RecoveryPassword> GetByCodeAsync(string code)
    {
        var result = await _context.RecoveryPassword
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Code == code);
        
        if (result == null)
            throw new Exception("Código de recuperação não encontrado");

        return result;
    }

    public async Task DeleteAsync(RecoveryPassword recoveryPassword)
    {
        var rp = await _context.RecoveryPassword.FindAsync(recoveryPassword.Id);
        
        if (rp != null)
        {
            _context.RecoveryPassword.Remove(rp);
            await _context.SaveChangesAsync();
        }
    }
}