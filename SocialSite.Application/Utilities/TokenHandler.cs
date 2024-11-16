using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialSite.Application.Dtos.Account;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Application.Utilities;

public sealed class TokenHandler
{
	private readonly IAccountService _accountService;
	private readonly IOptions<JwtSetup> _options;
	private readonly IDateTimeProvider _dateTimeProvider;

	public TokenHandler(IOptions<JwtSetup> options, IDateTimeProvider dateTimeProvider, IAccountService accountService)
	{
		_options = options;
		_dateTimeProvider = dateTimeProvider;
		_accountService = accountService;
	}
    
	public async Task<AuthTokensDto> CreateAuthTokens(int userId, bool extendRefreshValidity)
	{
		var userClaims = await _accountService.GetUserClaimsAsync(userId);
		var accessToken = GenerateAccessToken(userClaims);
		var refreshTokenPlain = CreateRefreshToken();
		
		var validHours = extendRefreshValidity ? _options.Value.ExtendedRefreshValidHours : _options.Value.RefreshValidHours;
		var expirationDate = _dateTimeProvider.GetDateTime().AddHours(validHours);
		
		await StoreRefreshTokenAsync(userId, refreshTokenPlain, expirationDate);
        
		return new AuthTokensDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshTokenPlain
		};
	}

	public async Task<AuthTokensDto> CreateAuthTokensFromRefreshToken(string refreshToken)
	{
		var storedRefreshToken = await RetrieveAndValidateRefreshTokenAsync(refreshToken);

		var userId = storedRefreshToken.UserId;
		await _accountService.DeleteRefreshTokenAsync(storedRefreshToken.Id);

		var userClaims = await _accountService.GetUserClaimsAsync(userId);
		var newAccessToken = GenerateAccessToken(userClaims);
		var newRefreshTokenPlain = CreateRefreshToken();
		
		await StoreRefreshTokenAsync(userId, newRefreshTokenPlain, storedRefreshToken.ExpirationDate);

		return new AuthTokensDto
		{
			AccessToken = newAccessToken,
			RefreshToken = newRefreshTokenPlain
		};
	}

	public async Task InvalidateRefreshTokenAsync(string refreshToken)
	{
		var storedRefreshToken = await RetrieveAndValidateRefreshTokenAsync(refreshToken);
		await _accountService.DeleteRefreshTokenAsync(storedRefreshToken.Id);
	}

	private async Task<RefreshToken> RetrieveAndValidateRefreshTokenAsync(string refreshToken)
	{
		var hashedToken = Hash(refreshToken);
		var storedRefreshToken = await _accountService.GetRefreshTokenAsync(hashedToken);

		if (storedRefreshToken is null || storedRefreshToken.ExpirationDate <= _dateTimeProvider.GetDateTime())
			throw new NotValidException("Invalid or expired refresh token.");

		return storedRefreshToken;
	}

	private string GenerateAccessToken(IEnumerable<Claim> userClaims)
	{
		var claims = userClaims.ToList();
		claims.Add(new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

		var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.AccessSecret));
		var jwtSecurityToken = new JwtSecurityToken(
			issuer: _options.Value.ValidIssuer,
			audience: _options.Value.ValidAudience,
			expires: _dateTimeProvider.GetDateTime().AddHours(_options.Value.AccessValidHours),
			claims: claims,
			signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
		);

		return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
	}

	private static string CreateRefreshToken()
	{
		return Guid.NewGuid().ToString();
	}

	private async Task StoreRefreshTokenAsync(int userId, string refreshTokenPlain, DateTime expirationDate)
	{
		var hashedToken = Hash(refreshTokenPlain);
		var refreshToken = new RefreshToken
		{
			UserId = userId,
			Token = hashedToken,
			ExpirationDate = expirationDate
		};
		await _accountService.CreateRefreshTokenAsync(refreshToken);
	}

	private string Hash(string token)
	{
		var tokenBytes = Encoding.UTF8.GetBytes(token + _options.Value.RefreshSecret);
		var hashedBytes = SHA256.HashData(tokenBytes);
		return Convert.ToBase64String(hashedBytes);
	}
}