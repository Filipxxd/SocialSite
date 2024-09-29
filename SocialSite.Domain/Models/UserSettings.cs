using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class UserSettings
{
    public bool AllowNonFriendMessages { get; set; }
    public FriendRequestSettingState FriendRequestSettingState { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }
}
