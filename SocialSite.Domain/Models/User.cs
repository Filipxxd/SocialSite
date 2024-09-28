using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class User : ChangeTracking, ISoftDeletable
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string GoogleId { get; set; } = string.Empty;
    public Role Role { get; set; }

    public bool AllowNonFriendMessages { get; set; }
    public FriendRequestSettingState FriendRequestSettingState { get; set; }

    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<Chat> Chats { get; set; } = [];
    public virtual ICollection<FriendRequest> FriendRequests { get; set; } = [];
    public virtual ICollection<Friendship> Friendships { get; set; } = [];
}
