namespace SocialSite.Domain.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;

    public DateTime SentAt { get; set; }

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int ChatId { get; set; }
    public virtual Chat? Chat { get; set; }

    public virtual ICollection<Image> Images { get; set; } = [];
}
