namespace SocialSite.Application.Dtos.Friends;

public class FriendRequestDto
{
	public int Id { get; set; }
	public string SenderFullname { get; set; } = default!;
	public DateTime SentAt { get; set; }
}