using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Posts;
using SocialSite.Domain.Filters;
using System.Net;
using ILogger = SocialSite.Domain.Utilities.ILogger;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/posts")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class PostsController : ApiControllerBase
{
	private readonly PostAppService _postAppService;

	public PostsController(PostAppService postAppService, ILogger logger) : base(logger)
	{
		_postAppService = postAppService;
	}

	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedDto<PostDto>))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	public async Task<IActionResult> GetAllPosts([FromQuery] PostFilter filter)
		=> await ExecuteAsync(() => _postAppService.GetAllPostsAsync(filter, GetCurrentUserId()));

	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task CreatePost(CreatePostDto dto)
		=> await ExecuteWithoutContentAsync(() => _postAppService.CreatePostAsync(dto, GetCurrentUserId()));

	[HttpDelete]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> DeletePost(int postId)
		=> await ExecuteWithoutContentAsync(() => _postAppService.DeletePostAsync(postId, GetCurrentUserId()));
}
