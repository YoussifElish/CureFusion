using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CureFusion.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(1000);

        builder.Property(a => a.CreatedIn)
            .HasDefaultValueSql("GETDATE()");



  
        builder.HasOne(q => q.User)
               .WithMany(u => u.Questions)
               .HasForeignKey(q => q.UserId)
               .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(q => q.RepliedByDoctor)
               .WithMany()
               .HasForeignKey(q => q.RepliedByDoctor)
               .OnDelete(DeleteBehavior.Restrict);

    }
}
