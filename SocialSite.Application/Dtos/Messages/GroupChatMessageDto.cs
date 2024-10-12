namespace SocialSite.Application.Dtos.Messages;

public sealed class GroupChatMessageDto
{
    public string Content { get; set; } = default!;
    public int SenderId { get; set; }
    public int GroupChatId { get; set; }
    public DateTime SentAt { get; set; }
}
