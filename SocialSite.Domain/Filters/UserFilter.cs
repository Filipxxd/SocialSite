using SocialSite.Domain.Filters.Base;

namespace SocialSite.Domain.Filters;

public sealed class UserFilter : SearchFilter
{
	public int CurrentUserId { get; set; }
	public bool? IsBanned { get; set; }
	public string? Role { get; set; }
}
