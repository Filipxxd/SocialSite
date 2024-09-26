using SocialSite.Domain.Models;
using SocialSite.Domain.Services.Filters;
using SocialSite.Domain.Utilities;

namespace SocialSite.Domain.Services;

public interface IUserService
{
    Task<Result<IEnumerable<User>>> GetAllAsync(UserFilter userFilter, PageFilter pageFilter);
    Task<Result<PaginationInfo>> GetPaginationInfoAsync(UserFilter userFilter, PageFilter pageFilter);
    Task<Result<User>> GetUserByIdAsync(int userId);

    Task<Result> CreateAsync(User user);
    Task<Result> UpdateAsync(User user);
}