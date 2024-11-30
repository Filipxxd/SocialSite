using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Mappers;
using SocialSite.Application.Utilities;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class UserAppService
{
    private readonly IUserService _userService;

    public UserAppService(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<PagedData<UserSearchDto>> GetFilteredUsersAsync(UserFilter filter, int currentUserId)
	{
		filter.CurrentUserId = currentUserId;
		var paginationInfo = await _userService.GetUsersPaginationInfoAsync(filter);
		var users = await _userService.GetUsersAsync(filter);

		return new(users.Map(), paginationInfo.RecordsCount, paginationInfo.TotalPages);
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
