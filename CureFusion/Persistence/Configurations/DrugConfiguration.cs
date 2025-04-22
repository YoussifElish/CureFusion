using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Persistence.Configurations;

public class DrugConfiguration : IEntityTypeConfiguration<Drug>

{
    public void Configure(EntityTypeBuilder<Drug> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Dosage).IsRequired().HasMaxLength(100);
        

    }
}
