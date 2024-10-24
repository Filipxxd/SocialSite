namespace SocialSite.Application.Dtos.Chats;

public sealed class ChatDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDirect { get; set; }
    public IEnumerable<ChatUserDto> Users { get; set; } = [];
    public IEnumerable<ChatMessageDto> Messages { get; set; } = [];
}
