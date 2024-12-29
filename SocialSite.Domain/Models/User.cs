using Microsoft.AspNetCore.Identity;
using SocialSite.Domain.Models.Enums;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace SocialSite.Domain.Models;

public class User : IdentityUser<int>
{
    public override string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Bio { get; set; }

    public bool AllowNonFriendChatAdd { get; set; } = true;
    public FriendRequestSetting FriendRequestSetting { get; set; }

    public string Fullname => $"{FirstName} {LastName}";

    public virtual ICollection<ChatUser> UserChats { get; set; } = [];
    public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = [];
    public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = [];
    public virtual ICollection<Friendship> FriendshipsInitiatedByUser { get; set; } = [];
    public virtual ICollection<Friendship> FriendshipsAcceptedByUser { get; set; } = [];
    public virtual ICollection<Post> Posts { get; set; } = [];
    public virtual ICollection<Report> Reports { get; set; } = [];

    public virtual string? ProfilePicturePath { get; set; }
}
