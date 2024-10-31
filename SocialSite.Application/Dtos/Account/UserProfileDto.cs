using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Account;

public class UserProfileDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string? ProfilePicturePath { get; set; } = default!;
    public string? Bio  { get; set; }
    
    public bool AllowNonFriendChatAdd { get; set; }
    public FriendRequestSettingState FriendRequestSettingState { get; set; }
    public PostVisibility PostVisibility { get; set; }
}