using AuthorizationServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServer.DataAccess.Context.Configurations;

public class UserApplicationConfiguration : IEntityTypeConfiguration<UserApplication>
{
    public void Configure(EntityTypeBuilder<UserApplication> builder)
    {
        builder.ToTable("UserApplications");
        
        builder.HasKey(ua => new { ua.UserId, ua.ApplicationId });

        builder
            .HasOne(ua => ua.User)
            .WithMany(u => u.UserApplications)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ua => ua.Application)
            .WithMany(a => a.UserApplications)
            .HasForeignKey(ua => ua.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(new
        {
            UserId = UserConfiguration.AdminUserId,
            ApplicationId = ApplicationConfiguration.AdminPanelId,
            Roles = new List<string> { "RootAdmin" }
        });
    }
}