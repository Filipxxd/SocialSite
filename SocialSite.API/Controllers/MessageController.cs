using System.Net;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Messages;

namespace SocialSite.API.Controllers;

[Route("message")]
public sealed class MessageController : ApiControllerBase
{
    private readonly MessageAppService _messageAppService;

    public MessageController(MessageAppService messageAppService)
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
