using SocialSite.Application.Dtos.Users;
using SocialSite.Domain.Models;

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
		                 || input.Friendships.Any(x => x.FriendId == currentUserId || x.UserId == currentUserId),
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
