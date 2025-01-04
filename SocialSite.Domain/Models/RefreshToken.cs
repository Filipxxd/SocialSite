namespace SocialSite.Domain.Models;

public class RefreshToken
{
	public int Id { get; set; }
	public string Token { get; set; } = default!;
	public DateTime DateCreated { get; set; }
	public DateTime ExpirationDate { get; set; }
	public int UserId { get; set; }
	public virtual User? User { get; set; }
}
