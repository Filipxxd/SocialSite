using SocialSite.Application.Dtos.Friends;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class FriendsAppService
{
    private readonly IFriendsService _friendsService;

    public FriendsAppService(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IEnumerable<FriendshipDto>> GetAllFriendshipsAsync(int currentUserId)
    {
	    var friendships = await _friendsService.GetAllFriendshipsAsync(currentUserId);

	    return friendships.Map(currentUserId);
    }
	
    public async Task<IEnumerable<FriendRequestDto>> GetAllFriendRequestsAsync(int currentUserId)
    {
	    var friendRequests = await _friendsService.GetAllFriendsRequestsAsync(currentUserId);

	    return friendRequests.Map();
    }
    
    public async Task SendFriendRequestAsync(CreateFriendRequestDto input, int currentUserId)
    {
        await _friendsService.SendFriendRequestAsync(input.Map(currentUserId));
    }
    
    public async Task ResolveFriendRequestAsync(ResolveFriendRequestDto input, int currentUserId)
    {
	    if (input.IsAccepted)
		    await _friendsService.AcceptFriendRequestAsync(input.Id, currentUserId);
	    else
		    await _friendsService.DeclineFriendRequestAsync(input.Id, currentUserId);
    }

    public async Task RemoveFriendAsync(FriendshipDto input, int currentUserId)
    {
	    await _friendsService.RemoveFriendAsync(input.FriendId, currentUserId);
    }
}