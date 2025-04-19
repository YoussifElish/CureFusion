using CureFusion.Abstactions.Consts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
      builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(50);
      builder.Property(x=>x.LastName).IsRequired().HasMaxLength(50);
        builder.OwnsMany(x => x.RefreshTokens).ToTable("RefreshTokens").WithOwner().HasForeignKey("UserId");

        //Default Data

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            FirstName = "CureFusion",
            LastName = "Admin",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
        });
    }
}
