namespace SocialSite.Application.Dtos.Users;

public sealed class UpdateProfileImageDto
{
	public string ImageData { get; set; } = default!;
	public string FileName { get; set; } = default!;
}