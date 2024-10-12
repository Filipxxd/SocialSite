using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAllDirectAsync(int receivingUserId, int currentUserId);
    Task<IEnumerable<Message>> GetAllGroupChatAsync(int groupChatId, int currentUserId);
    Task<Result> SendMessageAsync(Message message);
}
