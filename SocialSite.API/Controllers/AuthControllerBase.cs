using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Exceptions;
using SocialSite.Domain.Models;
using System.Security.Claims;

namespace SocialSite.API.Controllers;

public abstract class AuthControllerBase : ControllerBase
{
    private readonly UserManager<User> _userManager;

    protected AuthControllerBase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected async Task<User> GetCurrentUserAsync()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name) ?? "";
        return await _userManager.FindByNameAsync(userName) ?? throw new AuthException();
    }
}
