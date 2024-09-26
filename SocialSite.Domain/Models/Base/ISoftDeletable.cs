namespace SocialSite.Domain.Models.Base;

public interface ISoftDeletable
{
    public bool IsActive { get; set; }
}
