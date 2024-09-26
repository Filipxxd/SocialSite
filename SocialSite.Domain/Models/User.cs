using SocialSite.Domain.Models.Base;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class User : ChangeTracking, ISoftDeletable
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string GoogleId { get; set; }
    public Role Role { get; set; }
    public bool AllowNonFriendMessages { get; set; }
    public FriendRequestState FriendRequestState { get; set; }

    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Chat> Chats { get; set; } = [];
    public ICollection<FriendPair> Friends { get; set; } = [];
}
