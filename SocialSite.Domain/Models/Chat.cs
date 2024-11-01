namespace SocialSite.Domain.Models;

public class Chat
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int? OwnerId { get; set; }
    public virtual User? Owner { get; set; }

    public virtual Image? Image { get; set; }

    public bool IsDirect => OwnerId is null;
    
    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];
    public virtual ICollection<Message> Messages { get; set; } = [];
}
