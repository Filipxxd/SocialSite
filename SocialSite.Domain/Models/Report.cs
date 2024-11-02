using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class Report
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTime DateCreated { get; set; }
    public ReportType Type { get; set; }
    public ReportState State { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int PostId { get; set; }
    public virtual Post? Post { get; set; }
}
