using System.Reflection;
using AuthorizationServer.DataAccess.Context.Configurations;
using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthorizationServer.DataAccess.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Application> Applications => Set<Application>();

    public DbSet<UserApplication> UserApplications => Set<UserApplication>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<AuthorizationCode> AuthorizationCodes => Set<AuthorizationCode>();
    
    public AppDbContext() { }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AuthorizationServer.Web"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
            
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}