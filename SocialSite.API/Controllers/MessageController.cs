using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.API.Extensions;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Messages;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class MessageController : AuthControllerBase
{
    private readonly MessageAppService _messageAppService;

    public MessageController(MessageAppService messageAppService, UserManager<User> userManager) : base(userManager)
    {
        _messageAppService = messageAppService;
    }

    [HttpGet("get-all-direct")]
    [ProducesResponseType(typeof(Result<IEnumerable<DirectMessageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<DirectMessageDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllDirectMessages(int receivingUserId)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _messageAppService.GetAllPrivateMessages(receivingUserId, currentUser);

        return result.GetResponse();
    }

    [HttpGet("get-all-groupchat")]
    [ProducesResponseType(typeof(Result<IEnumerable<GroupChatMessageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<GroupChatMessageDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllGroupChatMessages(int groupChatId)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _messageAppService.GetAllGroupChatMessagesAsync(groupChatId, currentUser);

        return result.GetResponse();
    }

    [HttpPost("send-direct")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendDirect(NewDirectMessageDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _messageAppService.SendPrivateMessageAsync(dto, currentUser);

        return result.GetResponse(true);
    }

    [HttpPost("send-groupchat")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendGroupChat(NewGroupChatMessageDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _messageAppService.SendGroupChatMessageAsync(dto, currentUser);

        return result.GetResponse(true);
    }
}
