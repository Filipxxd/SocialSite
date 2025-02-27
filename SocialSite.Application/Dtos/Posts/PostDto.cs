using SocialSite.Application.Dtos.Comments;
using SocialSite.Application.Dtos.Images;

namespace SocialSite.Application.Dtos.Posts;

public sealed class PostDto
{
	public int PostId { get; set; }
	public string UserFullname { get; set; } = default!;
	public string? UserProfilePicturePath { get; set; }
	public string Content { get; set; } = default!;
	public DateTime DateCreated { get; set; }
	public bool IsReportable { get; set; }
	public bool IsDeletable { get; set; }
	public IEnumerable<CommentDto> Comments { get; set; } = [];
	public IList<ImageDto> Images { get; set; } = [];
}
