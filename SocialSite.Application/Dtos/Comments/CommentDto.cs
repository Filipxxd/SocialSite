namespace SocialSite.Application.Dtos.Comments;

public class CommentDto
{
	public int Id { get; set; }
	public string SenderFullname { get; set; } = default!;
	public string Content { get; set; } = default!;
}
