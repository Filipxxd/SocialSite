using SocialSite.Domain.Filters.Base;
using SocialSite.Domain.Filters.Enums;

namespace SocialSite.Domain.Filters;

public sealed class PostFilter : PageFilter
{
	public PostFilterVisibility Visibility { get; set; }
	public int? UserId { get; set; }
	public bool OnlyCurrentUser { get; set; }
}