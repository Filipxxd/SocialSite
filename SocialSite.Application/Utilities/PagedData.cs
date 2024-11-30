using System.Collections;

namespace SocialSite.Application.Utilities;


public class PagedData<T> : IEnumerable<T>
{
	private readonly IEnumerable<T> _items;

	public PagedData()
	{
		_items = [];
	}

	public PagedData(IEnumerable<T> items, int totalRecords, int totalPages)
	{
		_items = items;
		TotalRecords = totalRecords;
		TotalPages = totalPages;
	}

	public int TotalRecords { get; }
	public int TotalPages { get; }

	public IEnumerator<T> GetEnumerator()
	{
		return _items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
