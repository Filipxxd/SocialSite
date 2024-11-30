namespace SocialSite.Application.Dtos.Users;

public sealed class UserSearchDto
{
	public int UserId { get; set; }
	public string Fullname { get; set; } = default!;
	public string ProfilePicturePath { get; set; } = default!;
}