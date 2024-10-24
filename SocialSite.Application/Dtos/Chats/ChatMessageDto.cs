namespace SocialSite.Application.Dtos.Chats;

public sealed class ChatMessageDto
{
    public int Id { get; set; }
    public bool IsSentByCurrentUser { get; set; }
    public string SenderFullname { get; set; } = default!;
    public DateTime SentAt { get; set; }
    public string Content { get; set; } = default!;
}
