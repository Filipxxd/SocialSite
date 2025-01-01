namespace SocialSite.Domain.Models;

public class Image
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Path { get; set; } = default!;

	public int? PostId { get; set; }
	public Post? Post { get; set; }
}
