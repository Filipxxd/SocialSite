namespace SocialSite.Application.Dtos.Chats;

public sealed class ChatInfoDto
{
    public int ChatId { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDirect { get; set; }
}
