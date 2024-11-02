using System.ComponentModel;

namespace SocialSite.Domain.Models.Enums;

public enum FriendRequestSetting
{
    [Description("Anyone")] AnyOne,
    [Description("Only friends")] FriendsOfFriends,
    [Description("No one")] NoOne
}
