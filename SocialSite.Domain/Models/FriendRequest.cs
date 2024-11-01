using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class FriendRequest
{
    public int Id { get; set; }

    public FriendRequestState State { get; set; }

    public DateTime DateCreated { get; set; }

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }
}
