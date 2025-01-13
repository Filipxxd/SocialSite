using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;
using System.Net;
using ILogger = SocialSite.Domain.Utilities.ILogger;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/chats")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class ChatController : ApiControllerBase
{
    private readonly ChatAppService _chatAppService;

    public ChatController(ChatAppService chatAppService, ILogger logger) : base(logger)
    {
        _chatAppService = chatAppService;
    }

    [HttpGet("get-all")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatInfoDto>))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllChats()
        => await ExecuteAsync(() => _chatAppService.GetAllChatsAsync(GetCurrentUserId()));
    
    [HttpGet("get-chat")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetChat(int id)
        => await ExecuteAsync(() => _chatAppService.GetChatByIdAsync(id, GetCurrentUserId()));

    [HttpPost("create-direct")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateChat(CreateDirectChatDto dto)
        => await ExecuteAsync(() => _chatAppService.CreateDirectChatAsync(dto, GetCurrentUserId()));

    [HttpPost("create-group")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateChat(CreateGroupChatDto dto)
        => await ExecuteAsync(() => _chatAppService.CreateGroupChatAsync(dto, GetCurrentUserId()));
    
    [HttpPut("assign-group-users")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AssignGroupUsers(AssignGroupChatUsersDto dto)
        => await ExecuteWithoutContentAsync(() => _chatAppService.AssignUsersToGroupChatAsync(dto, GetCurrentUserId()));
}
