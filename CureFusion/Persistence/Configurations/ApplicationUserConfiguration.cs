using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
      builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(50);
      builder.Property(x=>x.LastName).IsRequired().HasMaxLength(50);
    }
}
