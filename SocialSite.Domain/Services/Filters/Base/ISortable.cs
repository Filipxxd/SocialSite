using SocialSite.Domain.Services.Filters.Enums;

namespace SocialSite.Domain.Services.Filters.Base;

public interface ISortable
{
    public SortOrder SortOrder { get; set; }
    public string SortProperty { get; set; }
}
