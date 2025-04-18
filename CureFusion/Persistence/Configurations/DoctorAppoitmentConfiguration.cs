using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Persistence.Configurations;

public class DoctorAppoitmentConfiguration : IEntityTypeConfiguration<DoctorAppoitment>
{
    public void Configure(EntityTypeBuilder<DoctorAppoitment> builder)
    {
        builder.Property(x => x.AvailableAt)
                  .IsRequired();

        builder.Property(x => x.DurationInMinutes)
               .IsRequired();

        builder.Property(x => x.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(x => x.SessionMode)
               .IsRequired();

        builder.Property(x => x.IsBooked)
               .IsRequired();

        builder.Property(x => x.Notes)
               .HasMaxLength(1000);

    }
}
