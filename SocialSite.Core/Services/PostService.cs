using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Filters.Enums;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class PostService : IPostService
{
    private readonly DataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PostService(DataContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

	public async Task<IEnumerable<Post>> GetAllPostsAsync(PostFilter filter, int currentUserId)
	{
		return await GetFilteredPosts(filter, currentUserId).OrderByDescending(p => p.DateCreated)
			.Skip(filter.PageSize * (filter.PageNumber - 1))
			.Take(filter.PageSize)
			.ToListAsync();
	}

	public async Task<int> GetRecordsCountAsync(PostFilter filter, int currentUserId)
	{
		return await GetFilteredPosts(filter, currentUserId).CountAsync();
	}
	
    public async Task<Post> GetPostByIdAsync(int postId, int currentUserId)
    {
	    return await _context.Posts.AsNoTracking()
		    .Include(p => p.Images)
		    .Include(p => p.User)
		    .Include(p => p.Comments.OrderByDescending(c => c.DateCreated))
		    .Where(p => 
			    p.Visibility == PostVisibility.Everyone || 
			    (p.Visibility == PostVisibility.FriendsOnly && 
			     _context.Friendships.Any(f => 
				     (f.UserInitiatedId == currentUserId && f.UserAcceptedId == p.UserId) || 
				     (f.UserAcceptedId == currentUserId && f.UserInitiatedId == p.UserId)
			     )))
		    .SingleOrDefaultAsync(p => p.Id == postId)
				?? throw new NotFoundException("Post was not found.");
    }
    
    public async Task CreatePostAsync(Post post)
    {
	    post.DateCreated = _dateTimeProvider.GetDateTime();
	    
        _context.Add(post);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdatePostAsync(Post updatedPost, int currentUserId)
    {
	    var post = await _context.Posts
		    .SingleOrDefaultAsync(p => p.Id == updatedPost.Id);
	    
	    if (post is null)
		    throw new NotFoundException("Post was not found.");
	    
	    if (post.UserId != currentUserId)
		    throw new NotValidException("Post can be updated only by the author.");
	    
	    post.Visibility = updatedPost.Visibility;
	    post.Images = updatedPost.Images;
	    post.Content = updatedPost.Content;
	    
	    await _context.SaveChangesAsync();
    }
    
    public async Task DeletePostAsync(int postId, int currentUserId)
    {
	    var post = await _context.Posts
		    .Include(e => e.Comments)
		    .SingleOrDefaultAsync(e => e.Id == postId);
	    
	    if (post is null)
		    throw new NotFoundException("Post was not found.");
	    
	    if (post.UserId != currentUserId)
			throw new NotValidException("Post can be deleted only by the author.");
		    
	    _context.Comments.RemoveRange(post.Comments);
	    _context.Images.RemoveRange(post.Images);
	    _context.Posts.Remove(post);

	    await _context.SaveChangesAsync();
    }

    private IQueryable<Post> GetFilteredPosts(PostFilter filter, int currentUserId)
    {
	    var query = _context.Posts.AsNoTracking()
		    .Include(p => p.Images)
		    .Include(p => p.User)
		    .Include(p => p.Comments.OrderByDescending(c => c.DateCreated))
		    .AsQueryable();

	    if (!filter.OnlyCurrentUser)
	    {
		    if (filter.Visibility == PostFilterVisibility.FromEveryone)
			    query = query.Where(p =>
				    p.Visibility == PostVisibility.Everyone
			    );
		    else
			    query = query.Where(p =>
				    p.Visibility != PostVisibility.Private && p.UserId != currentUserId &&
				    (p.User!.FriendshipsAcceptedByUser.Any(f => f.UserInitiatedId == currentUserId) || 
				     p.User.FriendshipsInitiatedByUser.Any(f => f.UserAcceptedId == currentUserId))
			    );
	    
		    if (filter.UserId != null)
			    query = query.Where(p =>
				    p.UserId == filter.UserId &&
				    (p.Visibility == PostVisibility.Everyone ||
				     (p.Visibility == PostVisibility.FriendsOnly &&
				      _context.Friendships.Any(f =>
					      (f.UserInitiatedId == currentUserId && f.UserAcceptedId == filter.UserId) ||
					      (f.UserAcceptedId == currentUserId && f.UserInitiatedId == filter.UserId)
				      ))));
	    }
	    else
	    {
		    query = query.Where(p => p.UserId == currentUserId);
	    }

	    return query;
    }
}
