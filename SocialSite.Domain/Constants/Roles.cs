namespace SocialSite.Domain.Constants;

public static class Roles
{
	public static readonly string[] AvailableRoles = [Moderator, User];
	
	public const string Admin = "Admin";
	public const string Moderator = "Moderator";
	public const string User = "User";
}