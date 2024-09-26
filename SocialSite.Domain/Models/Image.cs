namespace SocialSite.Domain.Models;

public class Image
{
    public int ImageId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }

    public int EntityId { get; set; }
    public virtual User? Owner { get; set; }
}
