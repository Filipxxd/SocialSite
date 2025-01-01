using SocialSite.Application.Dtos.Comments;
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
	
	public static PostDto Map(this Post input) => new()
	{
		PostId = input.Id,
		UserFullname = input.User!.Fullname,
		UserProfilePicturePath = input.User.ProfilePicturePath,
		Content = input.Content,
		DateCreated = input.DateCreated,
		Comments = input.Comments.Select(comment => new CommentDto
		{
			Id = comment.Id,
			SenderFullname = comment.User!.Fullname,
			Content = comment.Content
		})
	};
}