namespace SocialSite.Application.Dtos.Comments;

public class CommentDto
{
	public int CommentId { get; set; }
	public string SenderFullname { get; set; } = default!;
	public string? SenderProfilePicturePath { get; set; }
	public string Content { get; set; } = default!;
	public DateTime DateCreated { get; set; }
	public bool CanDelete { get; set; }
}
