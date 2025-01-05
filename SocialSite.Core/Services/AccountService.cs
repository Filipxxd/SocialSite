using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Constants;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;

namespace SocialSite.Core.Services;

public sealed class AccountService : IAccountService
{
	private readonly DataContext _dataContext;
	private readonly UserManager<User> _userManager;

	public AccountService(DataContext dataContext, UserManager<User> userManager)
	{
		_dataContext = dataContext;
		_userManager = userManager;
	}

	public async Task<int> LoginAsync(string userName, string password)
	{
		var user = await _userManager.FindByNameAsync(userName);
		if (user is null)
			throw new NotValidException("Invalid credentials.");

		var passwordValid = await _userManager.CheckPasswordAsync(user, password);
		if (!passwordValid)
			throw new NotValidException("Invalid credentials.");

		if (user.IsBanned)
			throw new NotValidException("Banned");
			
		return user.Id;
	}

	public async Task RegisterAsync(User user, string password)
	{
		var userExists = await _userManager.FindByNameAsync(user.UserName);

		if (userExists != null)
			throw new NotValidException("User with given username already exists.");

		var result = await _userManager.CreateAsync(user, password);
		if (!result.Succeeded)
			throw new NotValidException(string.Join(", ", result.Errors.Select(e => e.Description)));

		await _userManager.AddToRoleAsync(user, Roles.User);
	}
	
	public async Task<IEnumerable<Claim>> GetUserClaimsAsync(int userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString())
		    ?? throw new NotFoundException("User was not found.");

		var userRoles = await _userManager.GetRolesAsync(user);

		return
		[
			userRoles.Select(e => new Claim(AppClaimTypes.Role, e)).First(),
			new(AppClaimTypes.UserId, user.Id.ToString()),
			new(AppClaimTypes.Username, user.UserName)
		];
	}
	
	public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
	{
		await _dataContext.RefreshTokens.AddAsync(refreshToken);
		await _dataContext.SaveChangesAsync();
	}
	
	public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
	{
		return await _dataContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
	}
	
	public async Task DeleteRefreshTokenAsync(int tokenId)
	{
		var refreshToken = await _dataContext.RefreshTokens.FindAsync(tokenId);

		if (refreshToken != null)
		{
			_dataContext.RefreshTokens.Remove(refreshToken);
			await _dataContext.SaveChangesAsync();
		}
	}
}