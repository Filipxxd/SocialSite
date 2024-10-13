namespace SocialSite.Application.Dtos.Messages;

public sealed class NewGroupChatMessageDto
{
    public int GroupChatId { get; set; }
    public string Content { get; set; } = default!;
}
