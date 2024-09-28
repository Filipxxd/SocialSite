namespace SocialSite.Domain.Models;

public class Message
{
    public int MessageId { get; set; }
    public string Content { get; set; } = string.Empty;

    // obrazky

    public DateTime DateSent { get; set; }

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int ChatId { get; set; }
    public virtual Chat? Chat { get; set; }
}
