using login_system.Models;
using Microsoft.EntityFrameworkCore;

namespace login_system.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base (options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<RecoveryPassword> RecoveryPassword {get; set; }
}