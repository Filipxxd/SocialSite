using Microsoft.AspNetCore.Identity;
using SocialSite.Core.Constants;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;
using System.Security.Claims;

namespace SocialSite.Core.Services;

public sealed class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;

    public AccountService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetUserByIdAsync(int currentUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Claim>>> LoginAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return Result<IEnumerable<Claim>>.Fail(ResultErrors.NotValid, "Invalid credentials.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return Result<IEnumerable<Claim>>.Fail(ResultErrors.NotValid, "Invalid credentials.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = userRoles.Select(e => new Claim(ClaimTypes.Role, e)).ToList();

        claims.Add(new("fullname", user.FullName));

        return Result<IEnumerable<Claim>>.Success(claims);
    }

    public async Task<Result> RegisterAsync(User user, string password)
    {
        var userExists = await _userManager.FindByNameAsync(user.UserName);

        if (userExists != null)
            return Result.Fail(ResultErrors.NotValid, $"User with given username: '{user.UserName}' already exists.");

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return Result.Fail(ResultErrors.NotValid, result.Errors.Select(e => e.Description));

        return Result.Success();
    }
}
