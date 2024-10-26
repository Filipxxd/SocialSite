using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class Image
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Path { get; set; } = default!;

    public int EntityId { get; set; }
    public EntityName Entity { get; set; }
}
