using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Persistence.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
      builder.Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(a => a.CreatedIn)
            .HasDefaultValueSql("GETDATE()");


        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
