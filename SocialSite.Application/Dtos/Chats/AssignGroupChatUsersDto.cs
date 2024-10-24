namespace SocialSite.Application.Dtos.Chats;

public sealed class AssignGroupChatUsersDto
{
    public int GroupChatId { get; set; }
    public IEnumerable<int> UserIds { get; set; } = [];
}
