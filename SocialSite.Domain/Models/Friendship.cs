namespace SocialSite.Domain.Models;

public class Friendship
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int FriendId { get; set; }
    public virtual User? Friend { get; set; }
}

