using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IChatService
{
    Task<IEnumerable<Chat>> GetAllChatsAsync(int currentUserId);
    Task<Chat> GetChatByIdAsync(int chatId, int currentUserId);
    Task<Chat> CreateChatAsync(Chat chat, int currentUserId);
    Task AssignUsersToGroupChatAsync(int groupChatId, IList<int> userIds, int currentUserId);
}
