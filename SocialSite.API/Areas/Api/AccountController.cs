using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.Net;
using ILogger = SocialSite.Domain.Utilities.ILogger;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/account")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class AccountController : ApiControllerBase
{
    private readonly AccountAppService _accountAppService;

    public AccountController(AccountAppService accountAppService, ILogger logger) : base(logger)
    {
	    _accountAppService = accountAppService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Register(RegisterDto dto)
        => await ExecuteWithoutContentAsync(() => _accountAppService.RegisterAsync(dto));
    
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokensDto))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Login(LoginDto dto)
         => await ExecuteAsync(() => _accountAppService.LoginAsync(dto));
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokensDto))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
	    => await ExecuteAsync(() => _accountAppService.RefreshTokenAsync(dto));
    
    [HttpPost("logout")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Logout(RefreshTokenDto dto)
	    => await ExecuteWithoutContentAsync(() => _accountAppService.LogoutAsync(dto));
}
