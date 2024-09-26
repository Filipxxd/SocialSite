namespace SocialSite.Domain.Models;

public class Message
{
    public int MessageId { get; set; }
    public string Content { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }
}
