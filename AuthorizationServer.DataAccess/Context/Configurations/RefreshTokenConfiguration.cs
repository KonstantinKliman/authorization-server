using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();
        
        builder.Property(rt => rt.CreatedAt)
            .IsRequired();
        
        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(rt => rt.Application)
            .WithMany(a => a.RefreshTokens)
            .HasForeignKey(rt => rt.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(rt => rt.UserId);
        builder.HasIndex(rt => rt.ApplicationId);
        builder.HasIndex(rt => rt.ExpiresAt);
        builder.HasIndex(rt => new { rt.UserId, rt.ApplicationId });
    }
}