using System.Net;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Friends;

namespace SocialSite.API.Controllers;

[Microsoft.AspNetCore.Components.Route("friends")]
public class FriendController : ApiControllerBase
{
    private readonly FriendsAppService _friendsAppService;

    public FriendController(FriendsAppService friendsAppService)
    {
        _friendsAppService = friendsAppService;
    }

    [HttpPost("send-request")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SendFriendRequest(CreateFriendRequestDto dto)
        => await ExecuteWithoutContentAsync(() => _friendsAppService.SendFriendRequestAsync(dto, GetCurrentUserId()));
}