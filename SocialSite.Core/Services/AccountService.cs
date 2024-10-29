using Microsoft.AspNetCore.Identity;
using SocialSite.Core.Constants;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;
using System.Security.Claims;
using SocialSite.Core.Exceptions;

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

    public async Task<IEnumerable<Claim>> LoginAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            throw new NotValidException("Invalid credentials.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            throw new NotValidException("Invalid credentials.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = userRoles.Select(e => new Claim(ClaimTypes.Role, e)).ToList();

        claims.Add(new(AppClaimTypes.UserId, user.Id.ToString()));
        claims.Add(new(AppClaimTypes.Fullname, user.Fullname));

        return claims;
    }

    public async Task RegisterAsync(User user, string password)
    {
        var userExists = await _userManager.FindByNameAsync(user.UserName);

        if (userExists != null)
            throw new NotValidException($"User with given username: '{user.UserName}' already exists.");

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new NotValidException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
