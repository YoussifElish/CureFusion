using CureFusion.Domain.Entities;
using CureFusion.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CureFusion.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.Specialization)
                .IsRequired()
                .HasMaxLength(50);

        builder.Property(d => d.Bio)
                .HasMaxLength(1500);

        builder.Property(d => d.YearsOfExperience)
            .IsRequired();


        builder.Property(d => d.IsActive)
              .HasDefaultValue(true);

        builder.Property(d => d.accountStatus).HasDefaultValue(AccountStatus.Pending);


        //builder.Property(d => d.CertificationDocumentUrl)
        //    .HasMaxLength(500);

        builder.Property(d => d.Rating)
            .HasDefaultValue(0.0);

        builder.Property(d => d.TotalReviews)
            .HasDefaultValue(0);
    }
}
