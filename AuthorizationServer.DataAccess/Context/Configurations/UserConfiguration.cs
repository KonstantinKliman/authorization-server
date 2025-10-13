using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public static readonly Guid AdminUserId = Guid.Parse("15c25363-9a63-423c-b4b2-2e1acd1107cc");
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        builder
            .HasMany(u => u.Applications)
            .WithMany(a => a.Users)
            .UsingEntity<UserApplication>();
            

        builder.HasData(new User()
        {
            Id = AdminUserId,
            Name = "Admin",
            PasswordHash =
                "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1", //gM5RE7ecPAMnAJc54fuXJPEMq3N9ne5v
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}