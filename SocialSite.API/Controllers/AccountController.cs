using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Extensions;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Utilities;

namespace SocialSite.API.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AccountController : ControllerBase
{
    private readonly AccountAppService _accountAppService;

    public AccountController(AccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await _accountAppService.RegisterAsync(model);

        return result.GetResponse(true);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _accountAppService.LoginAsync(model);

        return result.GetResponse(true);
    }
}
