using SocialSite.Application.Dtos.Posts;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Reports;

public sealed class ReportDto
{
	public int ReportId { get; set; } 
	public string Content { get; set; } = default!;
	public ReportType Type { get; set; }
	public PostDto Post { get; set; } = default!;
	public DateTime DateCreated { get; set; }
	public string ReporterFullname { get; set; } = default!;
	public string? ReporterProfilePicturePath { get; set; }
}