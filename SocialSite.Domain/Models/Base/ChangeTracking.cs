namespace SocialSite.Domain.Models.Base;

public abstract class ChangeTracking
{
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public int CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public int UpdatedById { get; set; }
    public User? UpdatedBy { get; set; }
}
