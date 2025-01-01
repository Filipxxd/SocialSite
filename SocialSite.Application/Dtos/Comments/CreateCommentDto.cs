namespace SocialSite.Application.Dtos.Comments;

public sealed class CreateCommentDto
{
	public int PostId { get; set; }
	public string Content { get; set; } = default!;
}