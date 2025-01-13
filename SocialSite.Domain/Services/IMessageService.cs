using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IMessageService
{
    Task SendMessageAsync(Message message);
    Task<Message> GetMessageByIdAsync(int messageId, int currentUserId);
    Task DeleteMessageAsync(int messageId, int currentUserId);
}
