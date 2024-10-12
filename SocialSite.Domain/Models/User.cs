using Microsoft.AspNetCore.Identity;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class User : IdentityUser<int>
{
    public override string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Bio { get; set; }

    public bool AllowNonFriendMessages { get; set; } = true;
    public FriendRequestSettingState FriendRequestSettingState { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
    public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = [];
    public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = [];
    public virtual ICollection<Friendship> Friendships { get; set; } = [];

    public virtual ICollection<Message> SentMessages { get; set; } = [];
    public virtual ICollection<Message> ReceivedMessages { get; set; } = [];
}
