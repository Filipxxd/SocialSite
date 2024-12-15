using SocialSite.Domain.Filters.Enums;

namespace SocialSite.Domain.Filters.Base;

public interface ISortable
{
    public SortOrder SortOrder { get; set; }
    public string SortProperty { get; set; }
}
