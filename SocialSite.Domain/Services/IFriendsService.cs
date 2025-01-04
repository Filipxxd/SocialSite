using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IFriendsService
{
	Task<IEnumerable<Friendship>> GetAllFriendshipsAsync(int currentUserId);
	Task<IEnumerable<FriendRequest>> GetAllFriendsRequestsAsync(int currentUserId);
	Task SendFriendRequestAsync(FriendRequest request);
	Task RevokeFriendRequestAsync(int receiverId, int currentUserId);
	Task AcceptFriendRequestAsync(int requestId, int currentUserId);
	Task DeclineFriendRequestAsync(int requestId, int currentUserId);
	Task RemoveFriendAsync(int friendId, int currentUserId);
}