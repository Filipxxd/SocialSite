namespace SocialSite.Domain.Filters.Base;

public abstract class PageFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
