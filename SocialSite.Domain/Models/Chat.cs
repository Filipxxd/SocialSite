using SocialSite.Domain.Models.Base;

namespace SocialSite.Domain.Models;

public class Chat : ChangeTracking, ISoftDeletable
{
    public int ChatId { get; set; }
    public bool IsDirect { get; set; }
    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public int OwnerId { get; set; }
    public virtual User? Owner { get; set; }

    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Message> Messages { get; set; } = [];
}
