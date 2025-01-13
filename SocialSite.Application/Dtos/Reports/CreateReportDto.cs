using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Reports;

public sealed class CreateReportDto
{
	public int PostId { get; set; }
	public string Content { get; set; } = default!;
	public ReportType Type { get; set; }
}