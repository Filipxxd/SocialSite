using SocialSite.Application.Dtos.Friends;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class FriendMappingExtensions
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
		    FriendId = friendship.UserAcceptedId == currentUserId ? friendship.UserInitiated!.Id : friendship.UserAccepted!.Id,
		    FriendFullname = friendship.UserAcceptedId == currentUserId ? friendship.UserInitiated!.Fullname : friendship.UserAccepted!.Fullname,
		    ProfilePicturePath = friendship.UserAcceptedId == currentUserId ? friendship.UserInitiated!.ProfilePicturePath : friendship.UserAccepted!.ProfilePicturePath,
		    FriendsSince = friendship.DateCreated
	    });
    
    public static IEnumerable<FriendRequestDto> Map(this IEnumerable<FriendRequest> input) 
	    => input.Select(friendRequest => new FriendRequestDto
	    {
		    FriendRequestId = friendRequest.Id,
		    SenderFullname = friendRequest.Sender!.Fullname,
		    ProfilePicturePath = friendRequest.Sender.ProfilePicturePath,
		    SentAt = friendRequest.DateCreated
	    });
}