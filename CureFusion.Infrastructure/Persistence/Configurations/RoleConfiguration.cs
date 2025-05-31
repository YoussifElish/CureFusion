using CureFusion.Domain.Abstactions.Consts;
using CureFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Infrastructure.Persistence.EntitiesConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //Default Data
            builder.HasData([
                new ApplicationRole {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrenyStamp,
                },
                 new ApplicationRole {
                    Id = DefaultRoles.MemberRoleId,
                    Name = DefaultRoles.Member,
                    NormalizedName = DefaultRoles.Member.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.MemberRoleConcurrenyStamp,
                    IsDefault = true,
                },
                 new ApplicationRole {
                    Id = DefaultRoles.DoctorRoleId,
                    Name = DefaultRoles.Doctor,
                    NormalizedName = DefaultRoles.Doctor.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.DoctorConcurrenyStamp,
                    IsDefault = false,
                }
                ]);
        }
    }
}
