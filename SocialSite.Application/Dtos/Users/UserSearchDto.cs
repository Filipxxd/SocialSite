namespace SocialSite.Application.Dtos.Users;

public sealed class UserSearchDto
{
	public int UserId { get; set; }
	public bool IsBanned { get; set; }
	public string Role { get; set; } = default!;
	public string Username { get; set; } = default!;
	public string Fullname { get; set; } = default!;
	public string? ProfilePicturePath { get; set; }
}