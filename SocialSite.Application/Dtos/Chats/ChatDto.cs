namespace SocialSite.Application.Dtos.Chats;

public sealed class ChatDto
{
    public int ChatId { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDirect { get; set; }
    public IEnumerable<ChatUserDto> Users { get; set; } = [];
    public IEnumerable<ChatMessageDto> Messages { get; set; } = [];
}
