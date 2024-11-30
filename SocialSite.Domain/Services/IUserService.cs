using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IUserService
{
	Task<IEnumerable<User>> GetUsersAsync(UserFilter filter, PageFilter pageFilter);
	Task<PaginationInfo> GetUsersPaginationInfoAsync(UserFilter filter, PageFilter pageFilter);
    Task<User> GetProfileInfoAsync(int userId);
    Task<User> UpdateProfileInfoAsync(User user);
}
