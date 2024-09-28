namespace SocialSite.Domain.Models;

public class Image
{
    public int ImageId { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public int EntityId { get; set; }
    public virtual User? Owner { get; set; }
}
