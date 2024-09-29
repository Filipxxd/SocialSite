namespace SocialSite.Domain.Models;

public class Message
{
    public int MessageId { get; set; }
    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int? ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }

    public int? GroupChatId { get; set; }
    public virtual GroupChat? GroupChat { get; set; }
}
