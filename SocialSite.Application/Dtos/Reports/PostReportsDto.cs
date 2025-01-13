namespace SocialSite.Application.Dtos.Reports;

public sealed class PostReportsDto
{
	public int PostId { get; set; }
	public string UserFullname { get; set; } = default!;
	public string? UserProfilePicturePath { get; set; }
	public string Content { get; set; } = default!;
	public DateTime DateCreated { get; set; }
	public IEnumerable<ReportDto> Reports { get; set; } = [];
}