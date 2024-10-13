namespace SocialSite.Application.Dtos.Chats;

public sealed class GroupChatDto
{
    public int GroupChatId { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<GroupChatUserDto> Users { get; set; } = [];
}
