using CureFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Infrastructure.Persistence.Configurations;

public class DrugConfiguration : IEntityTypeConfiguration<Drug>

{
    public void Configure(EntityTypeBuilder<Drug> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Dosage).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1500);


    }
}
