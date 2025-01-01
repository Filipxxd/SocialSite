using SocialSite.Domain.Models.Enums;

namespace SocialSite.Domain.Models;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTime DateCreated { get; set; }
    public PostVisibility Visibility { get; set; }
    
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = [];
    public virtual ICollection<Image> Images { get; set; } = [];
}
