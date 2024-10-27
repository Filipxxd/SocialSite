using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Extensions;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Account;
using System.Net;

namespace SocialSite.API.Controllers;

[ApiController]
[Route("account")]
public sealed class AccountController : ControllerBase
{
    private readonly AccountAppService _accountAppService;

    public AccountController(AccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await _accountAppService.RegisterAsync(model);

        return result.GetResponse(true);
    }

    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TokenDto))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _accountAppService.LoginAsync(model);

        return Ok(result);
    }
}
