namespace SocialSite.Domain.Filters.Base;

public class SearchFilter : PageFilter 
{
    public string SearchTerm { get; set; } = string.Empty;
}
