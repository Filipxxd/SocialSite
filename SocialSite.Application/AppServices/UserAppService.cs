using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Images;
using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Application.AppServices;

public sealed class UserAppService
{
    private readonly IUserService _userService;
    private readonly IFriendsService _friendsService;
    private readonly IFileHandler _fileHandler;

    public UserAppService(IUserService userService, IFriendsService friendsService, IFileHandler fileHandler)
    {
	    _userService = userService;
	    _friendsService = friendsService;
	    _fileHandler = fileHandler;
    }

	public async Task<UserProfileDto> GetUserProfileAsync(string username, int currentUserId)
	{
		var user = await _userService.GetUserProfileAsync(username);
		
		return user.Map(currentUserId);
	}
    
	public async Task<PagedDto<UserSearchDto>> GetFilteredUsersAsync(string searchTerm, int currentUserId)
	{
		var filter = new UserFilter
		{
			SearchTerm = searchTerm,
			CurrentUserId = currentUserId
		};

		var paginationInfo = await _userService.GetUsersPaginationInfoAsync(filter);
		var users = await _userService.GetUsersAsync(filter);

		return new(users.Map(), paginationInfo.RecordsCount);
	}
    
    public async Task<MyProfileDto> GetProfileInfoAsync(int currentUserId)
    {
        var user = await _userService.GetProfileInfoAsync(currentUserId);
        return user.Map();
    }
    
    public async Task<MyProfileDto> UpdateProfileInfoAsync(UpdateProfileDto dto, int currentUserId)
    {
        var user = await _userService.UpdateProfileInfoAsync(dto.Map(currentUserId));
        return user.Map();
    }
    
    public async Task UpdateProfileImageAsync(ImageDto dto, int currentUserId)
    {
	    var user = await _userService.GetProfileInfoAsync(currentUserId);
	    var oldPath = user.ProfilePicturePath;
	    var path = await _fileHandler.SaveAsync(Convert.FromBase64String(dto.Base64), dto.Name);
	    
	    await _userService.UpdateProfileImageAsync(path, currentUserId);

	    if (oldPath != null)
		    _fileHandler.Delete(oldPath);
    }
}
