using CureFusion.Enums;

namespace CureFusion.Entities;

public class Question
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedIn { get; set; } = DateTime.Now;
    public ReactType React { get; set; }
    public ICollection<Answer> Answers { get; set; } =  [];
    public ApplicationUser User { get; set; } = default!;
    public string UserId { get; set; }
}

