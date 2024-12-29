namespace SocialSite.Application.Dtos.Friends;

public class FriendshipDto
{
	public int FriendId { get; set; }
	public string FriendFullname { get; set; } = default!;
	public string? ProfilePicturePath { get; set; }
	public DateTime FriendsSince { get; set; }
}