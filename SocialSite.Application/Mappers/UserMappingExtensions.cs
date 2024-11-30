using SocialSite.Application.Dtos.Users;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class UserMappingExtensions
{
	public static IEnumerable<UserSearchDto> Map(this IEnumerable<User> input)
		=> input.Select(user => new UserSearchDto
		{
			UserId = user.Id,
			Fullname = user.Fullname,
			ProfilePicturePath = "placeholder"
		});
	
    public static User Map(this UpdateProfileDto input, int currentUserId) => new()
    {
        Id = currentUserId,
        FirstName = input.Firstname,
        LastName = input.Lastname,
        Bio = input.Bio,
        AllowNonFriendChatAdd = input.AllowNonFriendChatAdd,
        FriendRequestSetting = input.FriendRequestSetting,
    };
    
    public static UserProfileDto Map(this User input) => new()
    {
        UserId = input.Id,
        Username = input.UserName,
        Firstname = input.FirstName,
        Lastname = input.LastName,
        ProfilePicturePath = input.ProfileImage?.Path,
        Bio = input.Bio,
        AllowNonFriendChatAdd = input.AllowNonFriendChatAdd,
        FriendRequestSetting = input.FriendRequestSetting,
    };
}
