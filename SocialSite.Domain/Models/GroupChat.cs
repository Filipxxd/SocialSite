namespace SocialSite.Domain.Models;

public class GroupChat
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int OwnerId { get; set; }
    public virtual User? Owner { get; set; }

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
    public virtual ICollection<Message> Messages { get; set; } = [];
}
