using Microsoft.AspNetCore.Identity;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.Security.Claims;

namespace SocialSite.Domain.Services;

public interface IAccountService
{
    Task<User> GetUserByIdAsync(int currentUserId);
    Task RegisterAsync(User user, string password);
    Task<IEnumerable<Claim>> LoginAsync(string userName, string password);
}
