using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class FriendRequest
{
    public int Id { get; set; }

    public FriendRequestStatus Status { get; set; }

    public DateTime RequestDate { get; set; }
    public DateTime? ResponseDate { get; set; }

    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }

    public int ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }
}
