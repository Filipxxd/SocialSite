using SocialSite.Application.Dtos.Comments;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class CommentMappingExtensions
{
	public static Comment Map(this CreateCommentDto input, int currentUserId) => new()
	{
		PostId = input.PostId,
		Content = input.Content,
		UserId = currentUserId
	};

	public static CommentDto Map(this Comment input, int currentUserId) => new()
	{
		CommentId = input.Id,
		SenderFullname = input.User!.Fullname,
		Content = input.Content,
		CanDelete = input.UserId == currentUserId || input.Post!.UserId == currentUserId,
		DateCreated = input.DateCreated,
		SenderProfilePicturePath = input.User.ProfilePicturePath
	};
}