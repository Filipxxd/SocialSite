using SocialSite.Domain.Filters.Base;

namespace SocialSite.Domain.Filters;

public sealed class UserFilter : FilterBase
{
	public int CurrentUserId { get; set; }
}