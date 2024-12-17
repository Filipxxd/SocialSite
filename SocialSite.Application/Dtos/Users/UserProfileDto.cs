using SocialSite.Application.Dtos.Users.Enums;

namespace SocialSite.Application.Dtos.Users;

public sealed class UserProfileDto
{
    public int UserId { get; set; }
    public string Fullname { get; set; } = default!;
    public string? ProfilePicturePath { get; set; } = default!;
    public string? Bio  { get; set; }
    
    public bool CanSendMessage { get; set; }
    public FriendState FriendState { get; set; }
}