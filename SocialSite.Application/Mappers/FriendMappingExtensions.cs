using SocialSite.Application.Dtos.Friends;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

public static class FriendMappingExtensions
{
    public static FriendRequest Map(this CreateFriendRequestDto input, int currentUserId)
    {
        return new FriendRequest
        {
            SenderId = currentUserId,
            ReceiverId = input.ReceiverId
        };
    }
}