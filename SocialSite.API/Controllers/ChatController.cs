using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;
using System.Net;

namespace SocialSite.API.Controllers;

[Route("chats")]
public sealed class ChatController : ApiControllerBase
{
    private readonly ChatAppService _chatAppService;

    public ChatController(UserManager<User> userManager, ChatAppService chatAppService) : base(userManager)
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
    public async Task<IActionResult> CreateChat([FromBody] CreateDirectChatDto dto)
        => await ExecuteAsync(() => _chatAppService.CreateDirectChatAsync(dto, GetCurrentUserId()));

    [HttpPost("create-group")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateChat([FromBody] CreateGroupChatDto dto)
        => await ExecuteAsync(() => _chatAppService.CreateGroupChatAsync(dto, GetCurrentUserId()));
    
    [HttpPut("assign-group-users")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AssignGroupUsers([FromBody] AssignGroupChatUsersDto dto)
        => await ExecuteWithoutContentAsync(() => _chatAppService.AssignUsersToGroupChatAsync(dto, GetCurrentUserId()));
}
