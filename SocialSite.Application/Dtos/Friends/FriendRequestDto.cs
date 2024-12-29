namespace SocialSite.Application.Dtos.Friends;

public class FriendRequestDto
{
	public int FriendRequestId { get; set; }
	public string SenderFullname { get; set; } = default!;
	public string? ProfilePicturePath { get; set; }
	public DateTime SentAt { get; set; }
}