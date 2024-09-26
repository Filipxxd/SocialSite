using SocialSite.Domain.Models.Base;

namespace SocialSite.Domain.Models;

public class Chat : ChangeTracking, ISoftDeletable
{
    public int GroupId { get; set; }
    public bool IsDirect { get; set; }
    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public int OwnerId { get; set; }
    public virtual User? Owner { get; set; }
}
