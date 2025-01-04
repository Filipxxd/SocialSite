namespace SocialSite.Application.Dtos.Users;

public sealed class ChangeUserRoleDto
{
	public int UserId { get; set; }
	public string Role { get; set; } = default!;
}