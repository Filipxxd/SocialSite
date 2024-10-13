using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IChatService
{
    Task<Result<IEnumerable<GroupChat>>> GetAllGroupChatsAsync(int currentUserId);
    Task<Result<GroupChat>> GetGroupChatByIdAsync(int groupChatId, int currentUserId);
    Task<Result<IEnumerable<User>>> GetAllDirectChatsAsync(int currentUserId);
    Task<Result> CreateGroupChatAsync(GroupChat chat);
    Task<Result> AddToGroupChatAsync(int groupChatId, int userId, int currentUserId);
    Task<Result> RemoveFromGroupChatAsync(int groupChatId, int userId, int currentUserId);
}
