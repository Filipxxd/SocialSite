using SocialSite.Application.Dtos.Posts;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class PostMappingExtensions
{
	public static Post Map(this CreatePostDto input, int currentUserId) => new()
	{
		Content = input.Content,
		Visibility = input.Visibility,
		UserId = currentUserId
	};

	public static PostDto Map(this Post input, int currentUserId) => new()
	{
		PostId = input.Id,
		UserFullname = input.User!.Fullname,
		UserProfilePicturePath = input.User.ProfilePicturePath,
		Content = input.Content,
		DateCreated = input.DateCreated,
		IsDeletable = input.UserId == currentUserId,
		IsReportable = input.Reports.All(r => r.UserId != currentUserId) && input.UserId != currentUserId,
		Comments = input.Comments.Select(comment => comment.Map(currentUserId))
	};
}