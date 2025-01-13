using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Images;
using SocialSite.Application.Dtos.Reports;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Application.AppServices;

public sealed class ReportsAppService
{
	private readonly IReportService _reportService;
	private readonly IFileHandler _fileHandler;
	
	public ReportsAppService(IReportService reportService, IFileHandler fileHandler)
	{
		_reportService = reportService;
		_fileHandler = fileHandler;
	}
	
	public async Task<PagedDto<ReportDto>> GetAllReportsAsync(ReportsFilter filter, int currentUserId)
	{
		var reports = await _reportService.GetAllReportsAsync(filter);
		var totalRecords = await _reportService.GetReportsCountAsync(filter);
		
		var dtos = new List<ReportDto>();
    
		foreach (var report in reports)
		{
			var reportDto = report.Map(currentUserId);

			foreach (var image in report.Post!.Images)
				reportDto.Post.Images.Add(new ImageDto
				{
					Name = image.Name,
					Base64 = Convert.ToBase64String(await _fileHandler.GetAsync(image.Path))
				});

			dtos.Add(reportDto);
		}

		return new PagedDto<ReportDto>(dtos, totalRecords);
	}
	
	public async Task CreateReportAsync(CreateReportDto dto, int currentUserId)
	{
		var report = dto.Map(currentUserId);
		await _reportService.CreateReportAsync(report);
	}

	public async Task ResolveReportAsync(int reportId, ResolveReportDto dto)
	{
		await _reportService.ResolveReportAsync(reportId, dto.Accepted);
	}
}