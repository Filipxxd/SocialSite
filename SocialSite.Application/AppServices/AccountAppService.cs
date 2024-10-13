using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialSite.Application.Dtos.Account;
using SocialSite.Core.Constants;
using SocialSite.Core.Utilities;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialSite.Application.AppServices;

public sealed class AccountAppService
{
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtSetup> _options;

    public AccountAppService(UserManager<User> userManager, IOptions<JwtSetup> options)
    {
        _userManager = userManager;
        _options = options;
    }

    public async Task<Result> RegisterAsync(RegisterDto dto)
    {
        var userExists = await _userManager.FindByNameAsync(dto.UserName);
        if (userExists != null)
            return Result.Fail(ResultErrors.NotValid, $"User with given username: '{dto.UserName}' already exists.");

        var user = dto.Adapt<User>();

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return Result.Fail(ResultErrors.NotValid, result.Errors.Select(e => e.Description));

        return Result.Success();
    }

    public async Task<Result<TokenDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null)
            return Result<TokenDto>.Fail(ResultErrors.NotValid, "Invalid credentials.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return Result<TokenDto>.Fail(ResultErrors.NotValid, "Invalid credentials.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = GetToken(authClaims);

        return Result<TokenDto>.Success(new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
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
