namespace CureFusion.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedIn { get; set; } = DateTime.Now;
    public int Upvotes { get; set; } = 0;
    public int Downvotes { get; set; } = 0;
    public string? RepliedByDoctorId { get; set; } // ID of the doctor who replied

    public ICollection<Answer> Answers { get; set; } = [];
    public ApplicationUser User { get; set; } = default!;
    public string UserId { get; set; }
    public ApplicationUser? RepliedByDoctor { get; set; }
}

