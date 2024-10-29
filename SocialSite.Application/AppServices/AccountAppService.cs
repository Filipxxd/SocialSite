﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialSite.Application.Dtos.Account;
using SocialSite.Application.Mappers;
using SocialSite.Core.Exceptions;
using SocialSite.Core.Utilities;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialSite.Application.AppServices;

public sealed class AccountAppService
{
    private readonly IAccountService _accountService;
    private readonly IOptions<JwtSetup> _options;

    public AccountAppService(UserManager<User> userManager, IOptions<JwtSetup> options, IAccountService accountService)
    {
        _options = options;
        _accountService = accountService;
    }

    public async Task<UserProfileDto> GetUserInfoAsync(int currentUserId)
    {
        return new();
    }
    
    public async Task RegisterAsync(RegisterDto dto)
    {
        var user = dto.Map();
        await _accountService.RegisterAsync(user, dto.Password);
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var claims = await _accountService.LoginAsync(dto.UserName, dto.Password);

        var token = GetToken([
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ..claims ]);

        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret));

        return new JwtSecurityToken(
            issuer: _options.Value.ValidIssuer,
            audience: _options.Value.ValidAudience,
            expires: DateTime.Now.AddHours(_options.Value.ValidHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
    }
}
