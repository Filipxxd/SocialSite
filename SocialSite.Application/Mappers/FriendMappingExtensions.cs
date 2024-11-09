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
    
    public static IEnumerable<FriendshipDto> Map(this IEnumerable<Friendship> input, int currentUserId) 
	    => input.Select(friendship => new FriendshipDto
	    {
		    FriendId = friendship.FriendId == currentUserId ? friendship.User!.Id : friendship.Friend!.Id,
		    FriendFullname = friendship.FriendId == currentUserId ? friendship.User!.Fullname : friendship.Friend!.Fullname,
		    FriendsSince = friendship.DateCreated
	    });
    
    public static IEnumerable<FriendRequestDto> Map(this IEnumerable<FriendRequest> input) 
	    => input.Select(friendRequest => new FriendRequestDto
	    {
		    Id = friendRequest.Id,
		    SenderFullname = friendRequest.Sender!.Fullname,
		    SentAt = friendRequest.DateCreated
	    });
}