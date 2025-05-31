using CureFusion.Domain.Abstactions.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Infrastructure.Persistence.EntitiesConfiguration
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            var permissions = Permissions.GetAllPermission();
            var adminClaims = new List<IdentityRoleClaim<string>>();
            for (var i = 0; i < permissions.Count; i++)
            {
                adminClaims.Add(new IdentityRoleClaim<string>
                {
                    Id = i + 1,
                    ClaimType = Permissions.Type,
                    ClaimValue = permissions[i],
                    RoleId = DefaultRoles.AdminRoleId
                });
            }
            builder.HasData(adminClaims);
        }
    }
}
