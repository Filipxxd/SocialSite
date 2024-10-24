using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.API.Extensions;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.Net;

namespace SocialSite.API.Controllers;

[ApiController]
[Authorize]
[Route("chats")]
public sealed class ChatController : AuthControllerBase
{
    private readonly ChatAppService _chatAppService;

    public ChatController(UserManager<User> userManager, ChatAppService chatAppService) : base(userManager)
    {
        _chatAppService = chatAppService;
    }

    [HttpGet("get-all")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatInfoDto>))]
    public async Task<IActionResult> GetAllChats()
    {
        var currentUser = await GetCurrentUserAsync();
        var dtos = await _chatAppService.GetAllChatsAsync(currentUser.Id);

        return Ok(dtos);
    }

    [HttpGet("get-chat")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    public async Task<IActionResult> GetChat(int id)
    {
        var currentUser = await GetCurrentUserAsync();
        var dto = await _chatAppService.GetChatByIdAsync(id, currentUser.Id);
        return Ok(dto);
    }

    [HttpPost("create-chat")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)] // ValidationProblems
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var chat = await _chatAppService.CreateChatAsync(dto, currentUser);

        return Ok(chat);
    }

    [HttpPut("assign-group-users")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignGroupUsers([FromBody] AssignGroupChatUsersDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.AssignUsersToGroupChatAsync(dto, currentUser);

        return result.GetResponse(true);
    }
}
