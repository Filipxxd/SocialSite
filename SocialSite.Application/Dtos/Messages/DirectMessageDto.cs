namespace SocialSite.Application.Dtos.Messages;

public sealed class DirectMessageDto
{
    public string Content { get; set; } = default!;
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime SentAt { get; set; }
}
