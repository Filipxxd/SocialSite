using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Users;

public class UpdateProfileDto
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string? Bio { get; set; }

    public bool AllowNonFriendChatAdd { get; set; } = true;
    public FriendRequestSetting FriendRequestSetting { get; set; }
}