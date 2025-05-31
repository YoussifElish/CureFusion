using CureFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(1000);

        builder.Property(a => a.CreatedIn)
            .HasDefaultValueSql("GETDATE()");

    }
}
