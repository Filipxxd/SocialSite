using SocialSite.Application.Dtos.Comments;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class CommentAppService
{
	private readonly ICommentService _commentService;

	public CommentAppService(ICommentService commentService)
	{
		_commentService = commentService;
	}
	
	public async Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, int currentUserId)
	{
		var comment = await _commentService.CreateCommentAsync(dto.Map(currentUserId));

		return comment.Map(currentUserId);
	}
	
	public async Task DeleteCommentAsync(int commentId, int currentUserId)
	{
		await _commentService.DeleteCommentAsync(commentId, currentUserId);
	}
}