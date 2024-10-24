using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Messages;
using SocialSite.Domain.Models;
using System.Net;

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

    [HttpPost("send")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> SendDirect(CreateMessageDto dto)
    {
        var currentUser = await GetCurrentUserAsync();
        var result = await _messageAppService.SendMessageAsync(dto, currentUser);

        return NoContent();
    }
}
