namespace SocialSite.Domain.Models;

public class Friendship
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }

    public int UserInitiatedId { get; set; }
    public virtual User? UserInitiated { get; set; }

    public int UserAcceptedId { get; set; }
    public virtual User? UserAccepted { get; set; }
}

