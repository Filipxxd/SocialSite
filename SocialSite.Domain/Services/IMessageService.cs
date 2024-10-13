using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IMessageService
{
    Task<Result<IEnumerable<Message>>> GetAllDirectAsync(int receivingUserId, int currentUserId);
    Task<Result<IEnumerable<Message>>> GetAllGroupChatAsync(int groupChatId, int currentUserId);
    Task<Result> SendMessageAsync(Message message);
}
