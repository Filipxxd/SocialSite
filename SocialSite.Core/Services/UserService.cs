using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
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
        // TODO: .AsNoTrackingWithIdentityResolution()
        return new();
    }

    public async Task<Result<PaginationInfo>> GetPaginationInfoAsync(UserFilter userFilter, PageFilter pageFilter)
    {
        return new();
    }

    public async Task<Result<User>> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u)
            .SingleOrDefaultAsync(u => u.Id == userId);
        
        if (user is null)
            return Result<User>.Fail(ResultErrors.NotFound, "User was not found.");
        
        return Result<User>.Success(user);
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
