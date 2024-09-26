using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class FriendPair
{
    public int FriendPairId { get; set; }

    public int RequesterId { get; set; }
    public User? Requester { get; set; }

    public int ReceiverId { get; set; }
    public User? Receiver { get; set; }

    public FriendStatus Status { get; set; }

    public DateTime RequestDate { get; set; }
    public DateTime? ResponseDate { get; set; }
}
