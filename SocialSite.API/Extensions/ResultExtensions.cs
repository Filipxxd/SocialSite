using Microsoft.AspNetCore.Mvc;
using SocialSite.Core.Constants;
using SocialSite.Domain.Utilities;

namespace SocialSite.API.Extensions;

internal static class ResultExtensions
{
    public static IActionResult GetResponse(this Result result, bool isCreate = false)
    {
        if (result.IsSuccess)
        {
            return isCreate ? new CreatedResult((string?)null, result) : new OkObjectResult(result);
        }

        if (result.Errors.ContainsKey(ResultErrors.NotFound))
        {
            return new NotFoundObjectResult(result);
        }

        if (result.Errors.ContainsKey(ResultErrors.NotValid))
        {
            return new BadRequestObjectResult(result);
        }

        if (result.Errors.ContainsKey(ResultErrors.NotAuthorized))
        {
            return new UnauthorizedObjectResult(result);
        }

        if (result.Errors.ContainsKey(ResultErrors.Concurrency))
        {
            return new ConflictObjectResult(result);
        }

        return new ObjectResult(result)
        {
            StatusCode = 500
        };
    }
}
