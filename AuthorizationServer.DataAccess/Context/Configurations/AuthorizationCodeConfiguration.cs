using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class AuthorizationCodeConfiguration : IEntityTypeConfiguration<AuthorizationCode>
{
    public void Configure(EntityTypeBuilder<AuthorizationCode> builder)
    {
        builder.HasKey(ac => ac.Id);
        
        builder.Property(ac => ac.Code)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.HasIndex(ac => ac.Code)
            .IsUnique();
        
        builder.Property(ac => ac.RedirectUri)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(ac => ac.ExpiresAt)
            .IsRequired();
        
        builder.Property(ac => ac.CreatedAt)
            .IsRequired(); // PostgreSQL функция
        
        builder.Property(ac => ac.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder
            .HasOne(ac => ac.User)
            .WithMany(u => u.AuthorizationCodes)
            .HasForeignKey(ac => ac.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(ac => ac.Application)
            .WithMany(a => a.AuthorizationCodes)
            .HasForeignKey(ac => ac.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(ac => ac.UserId);
        builder.HasIndex(ac => ac.ApplicationId);
        builder.HasIndex(ac => ac.ExpiresAt);
        builder.HasIndex(ac => ac.IsUsed);
    }
}