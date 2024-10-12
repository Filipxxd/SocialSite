namespace SocialSite.Domain.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;

    public DateTime SentAt { get; set; }

    public bool IsDirect => GroupChatId is null && ReceiverId != null;

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int? ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }

    public int? GroupChatId { get; set; }
    public virtual GroupChat? GroupChat { get; set; }
}
