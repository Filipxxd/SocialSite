using SocialSite.Domain.Services.Filters.Base;
using SocialSite.Domain.Services.Filters.Enums;

namespace SocialSite.Domain.Services;

public class UserFilter : FilterBase, ISortable
{
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    public string SortProperty { get; set; } = string.Empty;
}
