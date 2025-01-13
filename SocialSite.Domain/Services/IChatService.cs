using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IChatService
{
    Task<IEnumerable<Chat>> GetAllChatsAsync(int currentUserId);
    Task<Chat> GetChatByIdAsync(int chatId, int currentUserId);
    Task<Chat> CreateChatAsync(Chat chat, int currentUserId);
    Task AssignUsersToGroupChatAsync(int groupChatId, IList<int> userIds, int currentUserId);
    Task<bool> IsUserInChatAsync(int chatId, int userId);
}
