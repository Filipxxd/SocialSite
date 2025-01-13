using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Messages;
using System.Net;
using ILogger = SocialSite.Domain.Utilities.ILogger;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/messages")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class MessagesController : ApiControllerBase
{
	private readonly MessageAppService _messageAppService;

	public MessagesController(MessageAppService messageAppService, ILogger logger) : base(logger)
	{
		_messageAppService = messageAppService;
	}

	[HttpPost("send")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> SendMessage(CreateMessageDto dto)
		=> await ExecuteWithoutContentAsync(() => _messageAppService.SendMessageAsync(dto, GetCurrentUserId()));
}
