using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.API.Extensions;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class ChatController : AuthControllerBase
{
    private readonly ChatAppService _chatAppService;

    public ChatController(UserManager<User> userManager, ChatAppService chatAppService) : base(userManager)
    {
        _chatAppService = chatAppService;
    }

    [HttpGet("get-all-group")]
    [ProducesResponseType(typeof(IEnumerable<GroupChatInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGroupChats()
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.GetAllGroupChats(currentUser.Id);

        return result.GetResponse();
    }

    [HttpGet("get-group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupChatById([FromQuery] int groupChatId)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.GetGroupChatByIdAsync(groupChatId, currentUser.Id);

        return result.GetResponse();
    }

    [HttpGet("get-all-direct")]
    [ProducesResponseType(typeof(IEnumerable<DirectChatInfo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDirectChats()
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.GetAllDirectChatsAsync(currentUser.Id);

        return result.GetResponse();
    }

    [HttpPost("create-group")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroupChat([FromBody] NewGroupChatDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.CreateGroupChatAsync(dto, currentUser);

        return result.GetResponse(true);
    }

    [HttpPost("add-to-group")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddToGroupChat([FromBody] UserInGroupChat dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.AddToGroupChatAsync(dto, currentUser.Id);

        return result.GetResponse(true);
    }

    [HttpPost("remove-from-group")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveFromGroupChat([FromBody] UserInGroupChat dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _chatAppService.AddToGroupChatAsync(dto, currentUser.Id);

        return result.GetResponse();
    }
}
