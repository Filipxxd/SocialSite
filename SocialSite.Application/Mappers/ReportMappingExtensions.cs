using SocialSite.Application.Dtos.Reports;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class ReportMappingExtensions
{
	public static Report Map(this CreateReportDto dto, int currentUserId) => new Report
	{
		Content = dto.Content,
		Type = dto.Type,
		UserId = currentUserId,
		PostId = dto.PostId
	};
	
	public static ReportDto Map(this Report report, int currentUserId) => new ReportDto
	{
		ReportId = report.Id,
		Content = report.Content,
		Type = report.Type,
		Post = report.Post!.Map(currentUserId),
		DateCreated = report.DateCreated,
		ReporterFullname = report.User!.Fullname,
		ReporterProfilePicturePath = report.User.ProfilePicturePath
	};
}