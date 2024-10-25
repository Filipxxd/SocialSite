namespace SocialSite.Domain.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTime SentAt { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int PostId { get; set; }
    public virtual Post? Post { get; set; }
}
