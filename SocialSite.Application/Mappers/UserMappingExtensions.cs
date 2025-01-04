using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Dtos.Users.Enums;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Mappers;

internal static class UserMappingExtensions
{
	public static IEnumerable<UserSearchDto> Map(this IEnumerable<User> input) => input.Select(user => new UserSearchDto
		{
			Username = user.UserName,
			Fullname = user.Fullname,
			ProfilePicturePath = user.ProfilePicturePath
		});
	
	public static UserProfileDto Map(this User input, int currentUserId) => new()
	{
		UserId = input.Id,
		Fullname = input.Fullname,
		ProfilePicturePath = input.ProfilePicturePath,
		Bio = input.Bio,
		CanSendMessage = input.AllowNonFriendChatAdd 
		                 || input.FriendshipsInitiatedByUser.Any(x => x.UserAcceptedId == currentUserId || x.UserInitiatedId == currentUserId)
		                 || input.FriendshipsAcceptedByUser.Any(x => x.UserAcceptedId == currentUserId || x.UserInitiatedId == currentUserId),
		FriendState = input.FriendshipsInitiatedByUser.Any(x => x.UserAcceptedId == currentUserId) ||
		              input.FriendshipsAcceptedByUser.Any(x => x.UserInitiatedId == currentUserId)
						? FriendState.Friends
						: input.SentFriendRequests.Any(x => x.ReceiverId == currentUserId && x.State == FriendRequestState.Sent)
							? FriendState.RequestReceived
							: input.ReceivedFriendRequests.Any(
								x => x.SenderId == currentUserId && x.State == FriendRequestState.Sent)
								? FriendState.RequestSent
								: FriendState.CanSendRequest
	};
	
    public static User Map(this UpdateProfileDto input, int currentUserId) => new()
    {
        Id = currentUserId,
        FirstName = input.Firstname,
        LastName = input.Lastname,
        Bio = input.Bio,
        AllowNonFriendChatAdd = input.AllowNonFriendChatAdd,
        FriendRequestSetting = input.FriendRequestSetting,
    };
    
    public static MyProfileDto Map(this User input) => new()
    {
        UserId = input.Id,
        Username = input.UserName,
        Firstname = input.FirstName,
        Lastname = input.LastName,
        ProfilePicturePath = input.ProfilePicturePath,
        Bio = input.Bio,
        AllowNonFriendChatAdd = input.AllowNonFriendChatAdd,
        FriendRequestSetting = input.FriendRequestSetting,
    };
}
