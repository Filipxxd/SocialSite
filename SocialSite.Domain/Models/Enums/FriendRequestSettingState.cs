using System.ComponentModel;

namespace SocialSite.Domain.Models.Enums;

public enum FriendRequestSettingState
{
    [Description("Kdokoliv")] AnyOne,
    [Description("Pouze přátelé")] FriendsOfFriends,
    [Description("Nikdo")] NoOne
}
