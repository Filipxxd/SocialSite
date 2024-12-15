using System.Security.Claims;
using SocialSite.Domain.Models;

namespace SocialSite.Domain.Services;

public interface IAccountService
{
	Task RegisterAsync(User user, string password);
	Task<int> LoginAsync(string userName, string password);
	Task<IEnumerable<Claim>> GetUserClaimsAsync(int userId);
	Task CreateRefreshTokenAsync(RefreshToken refreshToken);
	Task<RefreshToken?> GetRefreshTokenAsync(string token);
	Task DeleteRefreshTokenAsync(int tokenId);
}