using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;
using System.Net;

namespace SocialSite.API.Controllers;

[Route("account")]
public sealed class AccountController : ApiControllerBase
{
    private readonly AccountAppService _accountAppService;

    public AccountController(AccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    [HttpGet("get-profile")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserProfileDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetProfile() 
        => await ExecuteAsync(() => _accountAppService.GetProfileInfoAsync(GetCurrentUserId()));
    
    [HttpPut("update-profile")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserProfileDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto) 
        => await ExecuteAsync(() => _accountAppService.UpdateProfileInfoAsync(dto, GetCurrentUserId()));
    
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Register(RegisterDto dto)
        => await ExecuteWithoutContentAsync(() => _accountAppService.RegisterAsync(dto));
    
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TokenDto))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Login(LoginDto dto)
         => await ExecuteAsync(() => _accountAppService.LoginAsync(dto));
}
