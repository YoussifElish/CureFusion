using CureFusion.Domain.Abstactions.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Infrastructure.Persistence.EntitiesConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            //Default Data
            builder.HasData([
                new IdentityUserRole<String> {
                   UserId = DefaultUsers.AdminId,
                   RoleId = DefaultRoles.AdminRoleId
                },

                ]);
        }
    }
}
