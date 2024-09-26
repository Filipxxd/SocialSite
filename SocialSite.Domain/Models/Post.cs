using SocialSite.Domain.Models.Base;

namespace SocialSite.Domain.Models;

public class Post : ChangeTracking, ISoftDeletable
{
    public int PostId { get; set; }
    public string Content { get; set; }

    public bool IsActive { get; set; }

    public int OwnerId { get; set; }
    public virtual User? Owner { get; set; }
}
