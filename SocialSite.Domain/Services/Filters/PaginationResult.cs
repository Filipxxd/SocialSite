namespace SocialSite.Domain.Services.Filters;

public class PaginationInfo(int recordsCount, int pageSize)
{
    public int RecordsCount { get; set; } = recordsCount;
    public int TotalPages { get; set; } = (int)Math.Ceiling(recordsCount / (double)pageSize);
}
