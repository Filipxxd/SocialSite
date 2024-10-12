namespace SocialSite.Domain.Models;

public class GroupUser
{
    public int Id { get; set; }

    public DateTime JoinedAt { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int GroupChatId { get; set; }
    public virtual GroupChat? GroupChat { get; set; }
}
