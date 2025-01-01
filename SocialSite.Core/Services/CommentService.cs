using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class CommentService : ICommentService
{
	private readonly DataContext _context;
	private readonly IDateTimeProvider _dateTimeProvider;

	public CommentService(DataContext context, IDateTimeProvider dateTimeProvider)
	{
		_context = context;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Comment> CreateCommentAsync(Comment comment)
	{
		var post = await _context.Posts.AsNoTracking()
			.SingleOrDefaultAsync(e => e.Id == comment.PostId);

		if (post is null)
			throw new NotFoundException("Post was not found.");

		comment.DateCreated = _dateTimeProvider.GetDateTime();
        
		_context.Comments.Add(comment);
		await _context.SaveChangesAsync();

		return await _context.Comments
			.Include(c => c.User)
			.Include(c => c.Post)
			.SingleAsync(c => c.Id == comment.Id);
	}

	public async Task DeleteCommentAsync(int commentId, int currentUserId)
	{
		var comment = await _context.Comments
			.Include(c => c.Post)
			.SingleOrDefaultAsync(c => c.Id == commentId);
	    
		if (comment is null)
			throw new NotFoundException("Comment was not found.");
	    
		if (comment.UserId != currentUserId && comment.Post!.UserId != currentUserId)
			throw new NotValidException("Comment can be deleted only by the author or the post owner.");

		_context.Comments.Remove(comment);
		await _context.SaveChangesAsync();
	}
}