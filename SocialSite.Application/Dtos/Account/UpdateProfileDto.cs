using SocialSite.Domain.Models.Enums;

namespace SocialSite.Application.Dtos.Account;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Bio { get; set; }

    public bool AllowNonFriendChatAdd { get; set; } = true;
    public FriendRequestSetting FriendRequestSetting { get; set; }
    public PostVisibility PostVisibility { get; set; }
}