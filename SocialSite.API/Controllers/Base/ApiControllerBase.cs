using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Models;
using System.Net;
using System.Security.Claims;

namespace SocialSite.API.Controllers.Base;

[Authorize]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly UserManager<User> _userManager;

    protected ApiControllerBase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> func)
        => await HandleRequestWithErrorHandling(async () =>
        {
            var result = await func();
            return Ok(result);
        });

    protected async Task<IActionResult> ExecuteWithoutContentAsync(Func<Task> func)
        => await HandleRequestWithErrorHandling(async () =>
        {
            await func();
            return NoContent();
        });

    protected async Task<User> GetCurrentUserAsync()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name) ?? "";
        return await _userManager.FindByNameAsync(userName) ?? throw new NotAuthorizedException("Unable to retrieve User from Claims");
    }

    private async Task<IActionResult> HandleRequestWithErrorHandling(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func();
        }
        catch (NotValidException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (NotAuthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
}
