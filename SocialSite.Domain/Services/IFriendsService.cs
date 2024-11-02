using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IFriendsService
{
    Task SendFriendRequestAsync(FriendRequest request);
}