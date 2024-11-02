using SocialSite.Application.Dtos.Friends;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public class FriendsAppService
{
    private readonly IFriendsService _friendsService;

    public FriendsAppService(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task SendFriendRequestAsync(CreateFriendRequestDto input, int currentUserId)
    {
        await _friendsService.SendFriendRequestAsync(input.Map(currentUserId));
    }
}