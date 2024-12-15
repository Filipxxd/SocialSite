using SocialSite.Application.Dtos.Account;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;
using TokenHandler = SocialSite.Application.Utilities.TokenHandler;

namespace SocialSite.Application.AppServices;

public sealed class AccountAppService
{
	private readonly TokenHandler _tokenHandler;
	private readonly IAccountService _accountService;
	
	public AccountAppService(TokenHandler tokenHandler, IAccountService accountService)
	{
		_tokenHandler = tokenHandler;
		_accountService = accountService;
	}
    
	public async Task RegisterAsync(RegisterDto dto)
	{
		var user = dto.Map();
		await _accountService.RegisterAsync(user, dto.Password);
	}

	public async Task<AuthTokensDto> LoginAsync(LoginDto dto)
	{
		var userId = await _accountService.LoginAsync(dto.UserName, dto.Password);
        
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