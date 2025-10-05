using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        builder.HasData(new User()
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            PasswordHash =
                "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1", //gM5RE7ecPAMnAJc54fuXJPEMq3N9ne5v
            CreatedAt = DateTime.UtcNow
        });
    }
}