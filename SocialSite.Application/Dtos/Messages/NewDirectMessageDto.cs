namespace SocialSite.Application.Dtos.Messages;

public sealed class NewDirectMessageDto
{
    public int ReceiverId { get; set; }
    public string Content { get; set; } = default!;
}
