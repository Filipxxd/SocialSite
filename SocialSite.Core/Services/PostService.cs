using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Data.EF.Extensions;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class PostService
{
    private readonly DataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PostService(DataContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task CreatePostAsync(Post post)
    {
        _context.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(int currentUserId)
    {
        return await _context.Posts.AsNoTracking()
            .IncludePostImages()
            .Include(p => p.Comments.OrderByDescending(c => c.DateCreated))
            .Include(p => p.User)
            .Where(p =>
                p.Visibility == PostVisibility.Everyone ||
                (p.Visibility == PostVisibility.FriendsOnly &&
                    _context.Friendships
                        .Where(f => f.UserId == currentUserId || f.FriendId == currentUserId)
                        .Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId)
                        .Contains(p.UserId))
            )
            .OrderByDescending(p => p.DateCreated)
            .ToListAsync();
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(e => e.Id == comment.PostId);

        if (post is null)
            throw new NotFoundException("Post was not found");

        comment.DateCreated = _dateTimeProvider.GetDateTime();
        
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return comment;
    }
}
