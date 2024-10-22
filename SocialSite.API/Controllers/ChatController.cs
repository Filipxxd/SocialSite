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
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Result<IEnumerable<ChatInfoDto>>))]
    public async Task<IActionResult> GetAllChats()
    {
        var currentUser = await GetCurrentUserAsync();
        var dtos = await _chatAppService.GetAllChatsAsync(currentUser.Id);

        return Ok(dtos);
    }

    [HttpGet("get-group")]
    [ProducesResponseType(typeof(Result<GroupChatDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<GroupChatDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupChatById([FromQuery] int groupChatId)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.GetGroupChatByIdAsync(groupChatId, currentUser.Id);

        return result.GetResponse();
    }

    [HttpPost("create-group")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroupChat([FromBody] NewGroupChatDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.CreateGroupChatAsync(dto, currentUser);

        return result.GetResponse(true);
    }

    [HttpPost("add-to-group")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddToGroupChat([FromBody] UserInGroupChat dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.AddToGroupChatAsync(dto, currentUser.Id);

        return result.GetResponse(true);
    }

    [HttpPost("remove-from-group")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveFromGroupChat([FromBody] UserInGroupChat dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.AddToGroupChatAsync(dto, currentUser.Id);

        return result.GetResponse();
    }
}
