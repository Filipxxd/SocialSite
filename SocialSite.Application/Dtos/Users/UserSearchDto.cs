namespace SocialSite.Application.Dtos.Users;

public sealed class UserSearchDto
{
	public string Username { get; set; } = default!;
	public string Fullname { get; set; } = default!;
	public string ProfilePicturePath { get; set; } = default!;
}