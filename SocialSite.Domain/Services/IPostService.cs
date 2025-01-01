using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IPostService
{
	Task<IEnumerable<Post>> GetAllPostsAsync(PostFilter filter, int currentUserId);
	Task<int> GetRecordsCountAsync(PostFilter filter, int currentUserId);
	Task<Post> GetPostByIdAsync(int postId, int currentUserId);
	Task CreatePostAsync(Post post);
	Task UpdatePostAsync(Post updatedPost, int currentUserId);
	Task DeletePostAsync(int postId, int currentUserId);
}