using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Models;
using System.Net;
using System.Security.Claims;
using SocialSite.Core.Constants;

namespace SocialSite.API.Controllers.Base;

[Authorize]
[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
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

    protected int GetCurrentUserId()
    {
        var userIdRaw = User.FindFirstValue(AppClaimTypes.UserId) 
               ?? throw new ArgumentNullException(nameof(AppClaimTypes.UserId), "UserId claim not found");
        return int.Parse(userIdRaw);
    }

    private async Task<IActionResult> HandleRequestWithErrorHandling(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func();
        }
        catch (NotValidException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Validation Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = ex.Message
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = ex.Message
            });
        }
        catch (NotAuthorizedException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Status = (int)HttpStatusCode.Unauthorized,
                Detail = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred."
            });
        }
    }
}
