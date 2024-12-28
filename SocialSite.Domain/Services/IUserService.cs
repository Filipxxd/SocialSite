using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IUserService
{
	Task<User> GetUserProfileAsync(string username);
	Task<IEnumerable<User>> GetUsersAsync(UserFilter filter);
	Task<PaginationInfo> GetUsersPaginationInfoAsync(UserFilter filter);
    Task<User> GetProfileInfoAsync(int userId);
    Task UpdateProfileImageAsync(string imagePath, int currentUserId);
    Task<User> UpdateProfileInfoAsync(User user);
}
