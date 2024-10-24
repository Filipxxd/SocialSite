namespace SocialSite.Application.Dtos.Chats;

public sealed class CreateChatDto
{
    public string? Name { get; set; }
    public bool IsDirect { get; set; }
    public IEnumerable<int> UserIds { get; set; } = [];
}
