namespace SocialSite.Application.Dtos.Chats;

public class CreateGroupChatDto
{
    public string Name { get; set; } = default!;
    public IEnumerable<int> RecipientUserIds { get; set; } = [];
}