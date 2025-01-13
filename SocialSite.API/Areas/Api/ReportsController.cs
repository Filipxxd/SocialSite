using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Reports;
using SocialSite.Domain.Filters;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/reports")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class ReportsController : ApiControllerBase
{
	private readonly ReportsAppService _reportsAppService;

	public ReportsController(ReportsAppService reportsAppService)
	{
		_reportsAppService = reportsAppService;
	}

	[HttpGet]
	[Authorize(Policy = AuthPolicies.ElevatedUsers)]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedDto<ReportDto>))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	public async Task<IActionResult> GetAllReports([FromQuery] ReportsFilter filter)
		=> await ExecuteAsync(() => _reportsAppService.GetAllReportsAsync(filter, GetCurrentUserId()));
	
	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task CreateReport(CreateReportDto dto)
		=> await ExecuteWithoutContentAsync(() => _reportsAppService.CreateReportAsync(dto, GetCurrentUserId()));
	
	[HttpPost("{reportId:int}")]
	[Authorize(Policy = AuthPolicies.ElevatedUsers)]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task ResolveReport(int reportId, ResolveReportDto dto)
		=> await ExecuteWithoutContentAsync(() => _reportsAppService.ResolveReportAsync(reportId, dto));
}