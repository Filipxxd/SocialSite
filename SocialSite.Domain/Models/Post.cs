using SocialSite.Domain.Models.Base;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class Post : ChangeTracking, ISoftDeletable
{
    public int PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public PostVisibility Visibility { get; set; }

    public bool IsActive { get; set; }

    // TODO: Many to many on users => userLikes
    // TODO: Many to many on users => comments

    public int OwnerId { get; set; }
    public virtual User? Owner { get; set; }
}
