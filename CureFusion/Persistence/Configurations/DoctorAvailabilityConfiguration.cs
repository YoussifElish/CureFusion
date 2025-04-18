using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Persistence.Configurations;

public class DoctorAvailabilityConfiguration : IEntityTypeConfiguration<DoctorAvailability>
{
    public void Configure(EntityTypeBuilder<DoctorAvailability> builder)
    {
        builder.Property(x => x.Date)
                  .IsRequired();

        builder.Property(x => x.From)
               .IsRequired();

        builder.Property(x => x.To)
               .IsRequired();

        builder.Property(x => x.SlotDurationInMinutes)
               .IsRequired();

        builder.Property(x => x.PricePerSlot)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(x => x.SessionMode)
               .IsRequired();

       
    }
}
