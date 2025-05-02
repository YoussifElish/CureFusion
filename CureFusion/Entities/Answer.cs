using CureFusion.Enums;

namespace CureFusion.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedIn { get; set; } = DateTime.Now;
    public ReactType React { get; set; }
    public string UserId { get; set; }=default!;
    public int QuestionId { get; set; }

    public Question Question { get; set; } =default!;
    public ApplicationUser User { get; set; } = default!;
   
}

