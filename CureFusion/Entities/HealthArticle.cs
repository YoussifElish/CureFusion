namespace CureFusion.Entities;

public class HealthArticle
{
    public int Id { get; set; }
    public string Category { get; set; }=string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedIn { get; set; }=DateTime.UtcNow;
    public Guid? HealthArticleImageId { get; set; }
    public UploadedFile HealthArticleImage { get; set; }
}
