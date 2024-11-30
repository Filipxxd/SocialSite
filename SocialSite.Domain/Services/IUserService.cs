using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IUserService
{
	Task<IEnumerable<User>> GetUsersAsync(UserFilter filter);
	Task<PaginationInfo> GetUsersPaginationInfoAsync(UserFilter filter);
    Task<User> GetProfileInfoAsync(int userId);
    Task<User> UpdateProfileInfoAsync(User user);
}
