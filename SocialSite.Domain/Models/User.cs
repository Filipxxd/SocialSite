using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }

    public UserSettings Settings { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
    public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = [];
    public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = [];
    public virtual ICollection<Friendship> Friendships { get; set; } = [];

    public virtual ICollection<Message> SentMessages { get; set; } = [];
    public virtual ICollection<Message> ReceivedMessages { get; set; } = [];
}
