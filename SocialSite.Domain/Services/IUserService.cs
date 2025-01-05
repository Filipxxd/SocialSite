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
    Task UpdateUserRoleAsync(int userId, string newRole);
    Task ToggleUserBanAsync(int userId, bool banned);
    Task ChangeUsernameAsync(int userId, string newUsername);
    Task<bool> UsernameExistsAsync(string username);
}
