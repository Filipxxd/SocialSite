using SocialSite.Application.Dtos.Images;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Posts;

public sealed class CreatePostDto
{
	public string Content { get; set; } = default!;
	public PostVisibility Visibility { get; set; }
	public IEnumerable<ImageDto> Images { get; set; } = [];
}
