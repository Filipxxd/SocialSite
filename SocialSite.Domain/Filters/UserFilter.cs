using SocialSite.Domain.Filters.Base;
using SocialSite.Domain.Filters.Enums;

namespace SocialSite.Domain.Filters;

public sealed class UserFilter : SearchFilter, ISortable
{
	public int CurrentUserId { get; set; }
	public string SortProperty { get; set; } = string.Empty;
	public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
