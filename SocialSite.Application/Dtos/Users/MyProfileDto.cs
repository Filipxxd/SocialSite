using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Users;

public class MyProfileDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string? ProfilePicturePath { get; set; } = default!;
    public string? Bio  { get; set; }
    
    public bool AllowNonFriendChatAdd { get; set; }
    public FriendRequestSetting FriendRequestSetting { get; set; }
}