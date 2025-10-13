using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public static readonly Guid AdminPanelId = Guid.Parse("94e7fd21-7b93-4633-9417-2cd54b0f315c");
    
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ClientId)
            .IsRequired()
            .HasMaxLength(100);
    
        builder.HasIndex(a => a.ClientId)
            .IsUnique();

        builder.Property(a => a.ClientSecret)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.RedirectUrls)
            .IsRequired();
        
        builder
            .HasMany(a => a.Users)
            .WithMany(u => u.Applications)
            .UsingEntity<UserApplication>();
        
        builder.HasData(new Application
        {
            Id = AdminPanelId,
            Name = "Admin Panel",
            ClientId = "admin-panel",
            ClientSecret = "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1", // тот же хеш что у Admin
            RedirectUrls = new List<string> 
            { 
                "http://localhost:4200/admin/callback",
            },
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}