using SocialSite.Application.Dtos.Account;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;
using TokenHandler = SocialSite.Application.Utilities.TokenHandler;

namespace SocialSite.Application.AppServices;

public sealed class AccountAppService
{
	private readonly TokenHandler _tokenHandler;
	private readonly IUserService _userService;
	
	public AccountAppService(TokenHandler tokenHandler, IUserService userService)
	{
		_tokenHandler = tokenHandler;
		_userService = userService;
	}
    
	public async Task RegisterAsync(RegisterDto dto)
	{
		var user = dto.Map();
		await _userService.RegisterAsync(user, dto.Password);
	}

	public async Task<AuthTokensDto> LoginAsync(LoginDto dto)
	{
		var userId = await _userService.LoginAsync(dto.UserName, dto.Password);
        
		return await _tokenHandler.CreateAuthTokens(userId, dto.RememberMe);
	}

	public async Task<AuthTokensDto> RefreshTokenAsync(RefreshTokenDto dto)
	{
		return await _tokenHandler.CreateAuthTokensFromRefreshToken(dto.Token);
	}

	public async Task LogoutAsync(RefreshTokenDto dto)
	{
		await _tokenHandler.InvalidateRefreshTokenAsync(dto.Token);
	}
}