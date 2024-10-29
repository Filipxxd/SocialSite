using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IMessageService
{
    Task SendMessageAsync(Message message);
}
