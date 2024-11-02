using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

public static class UserMappingExtensions
{
    public static User Map(this UpdateProfileDto input, int currentUserId) => new()
    {
        Id = currentUserId,
        FirstName = input.FirstName,
        LastName = input.LastName,
        Bio = input.Bio,
        AllowNonFriendChatAdd = input.AllowNonFriendChatAdd,
        FriendRequestSetting = input.FriendRequestSetting,
    };
    
    public static User Map(this RegisterDto input) => new()
    {
        UserName = input.UserName,
        FirstName = input.FirstName,
        LastName = input.LastName,
        SecurityStamp = Guid.NewGuid().ToString()
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
