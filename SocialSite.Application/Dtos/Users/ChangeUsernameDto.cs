namespace SocialSite.Application.Dtos.Users;

public sealed class ChangeUsernameDto
{
	public int UserId { get; set; }
	public string NewUsername { get; set; } = default!;
}