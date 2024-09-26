using SocialSite.Core.Validators;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Services.Filters;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class UserService(EntityValidator entityValidator, DataContext context) : IUserService
{
    private readonly DataContext _context = context;
    private readonly EntityValidator _entityValidator = entityValidator;

    public async Task<Result<IEnumerable<User>>> GetAllAsync(UserFilter userFilter, PageFilter pageFilter)
    {
        return new();
    }

    public async Task<Result<PaginationInfo>> GetPaginationInfoAsync(UserFilter userFilter, PageFilter pageFilter)
    {
        return new();
    }

    public async Task<Result<User>> GetUserByIdAsync(int userId)
    {
        return new();
    }

    public async Task<Result> CreateAsync(User user)
    {
        return new();
    }

    public async Task<Result> UpdateAsync(User user)
    {
        return new();
    }
}
