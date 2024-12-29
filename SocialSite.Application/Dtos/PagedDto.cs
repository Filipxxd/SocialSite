namespace SocialSite.Application.Dtos;

public sealed class PagedDto<T>
{
	public IEnumerable<T> Items { get; set; }
	public int TotalRecords { get; set; }

	public PagedDto(IEnumerable<T> items, int totalRecords)
	{
		Items = items;
		TotalRecords = totalRecords;
	}
}