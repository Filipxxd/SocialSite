namespace SocialSite.Application.Dtos.Chats;

public sealed class GroupChatUserDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = default!;
}
