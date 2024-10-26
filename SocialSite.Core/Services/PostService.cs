using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
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

    public async Task<IEnumerable<Post>> GetAllPostsFromFriends(int currentUserId)
    {
        return await _context.Posts.AsNoTracking()
            .Include(p => p.Comments.OrderByDescending(c => c.SentAt))
            .Where(p => _context.Friendships
                .Where(f => f.UserId == currentUserId || f.FriendId == currentUserId)
                .Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId)
                .Contains(p.UserId))
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task CreateComment(Comment comment)
    {
        var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(e => e.Id == comment.PostId);

        if (post is null)
            throw new NotFoundException();

        _context.Coments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task ReportPost()
    {

    }
}
