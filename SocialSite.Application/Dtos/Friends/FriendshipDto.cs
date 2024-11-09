namespace SocialSite.Application.Dtos.Friends;

public class FriendshipDto
{
	public int FriendId { get; set; }
	public string FriendFullname { get; set; } = default!;
	public DateTime FriendsSince { get; set; }
}