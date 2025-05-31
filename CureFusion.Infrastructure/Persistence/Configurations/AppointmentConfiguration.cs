using CureFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(x => x.AppointmentDate).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.DurationInMinutes).IsRequired();
    }
}
