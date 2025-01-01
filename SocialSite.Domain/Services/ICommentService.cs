using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface ICommentService
{
	Task<Comment> CreateCommentAsync(Comment comment);
	Task DeleteCommentAsync(int commentId, int currentUserId);
}