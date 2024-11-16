using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class UserAppService
{
    private readonly IUserService _userService;

    public UserAppService(IUserService userService)
	{
		_userService = userService;
	}

    public async Task<UserProfileDto> GetProfileInfoAsync(int currentUserId)
    {
        var user = await _userService.GetProfileInfoAsync(currentUserId);
        return user.Map();
    }
    
    public async Task<UserProfileDto> UpdateProfileInfoAsync(UpdateProfileDto dto, int currentUserId)
    {
        var user = await _userService.UpdateProfileInfoAsync(dto.Map(currentUserId));
        return user.Map();
    }
}
