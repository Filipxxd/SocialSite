using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Comments;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/comments")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class CommentsController : ApiControllerBase
{
    private readonly CommentAppService _commentAppService;

    public CommentsController(CommentAppService commentAppService)
    {
	    _commentAppService = commentAppService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CommentDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateComment(CreateCommentDto dto)
	    => await ExecuteAsync(() => _commentAppService.CreateCommentAsync(dto, GetCurrentUserId()));
    
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteComment(int commentId)
	    => await ExecuteWithoutContentAsync(() => _commentAppService.DeleteCommentAsync(commentId, GetCurrentUserId()));
}
