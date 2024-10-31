using Microsoft.AspNetCore.Identity;
using SocialSite.Core.Constants;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Data.EF.Extensions;

namespace SocialSite.Core.Services;

public sealed class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;
    
    public AccountService(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<User> GetProfileInfoAsync(int userId)
    {
        return await _context.Users.AsNoTracking()
                   .IncludeProfileImage()
                   .SingleOrDefaultAsync(u => u.Id == userId)
               ?? throw new NotFoundException("User was not found.");
    }

    public async Task<User> UpdateProfileInfoAsync(User user)
    {
        var currentUser = await _context.Users.FindAsync(user.Id)
                          ?? throw new NotFoundException("User was not found.");

        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Bio = user.Bio;
        currentUser.AllowNonFriendChatAdd = user.AllowNonFriendChatAdd;
        currentUser.FriendRequestSettingState = user.FriendRequestSettingState;
        currentUser.PostVisibility = user.PostVisibility;

        await _context.SaveChangesAsync();

        return currentUser;
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
            throw new NotValidException($"User with given username already exists.");

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new NotValidException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
