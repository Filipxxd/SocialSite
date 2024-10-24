namespace SocialSite.Application.Dtos.Messages;

public sealed class CreateMessageDto
{
    public int ChatId { get; set; }
    public string Content { get; set; } = default!;
}
