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
}