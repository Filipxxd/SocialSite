using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IReportService
{
	Task<IEnumerable<Report>> GetAllReportsAsync(ReportsFilter filter);
	Task<int> GetReportsCountAsync(ReportsFilter filter);
	Task CreateReportAsync(Report dto);
	Task ResolveReportAsync(int reportId, bool accepted);
}