using System.Net;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Friends;

namespace SocialSite.API.Controllers;

[Route("friends")]
public class FriendsController : ApiControllerBase
{
    private readonly FriendsAppService _friendsAppService;

    public FriendsController(FriendsAppService friendsAppService)
    {
        _friendsAppService = friendsAppService;
    }

    [HttpGet("get-all-friends")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FriendshipDto>))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllFriendships()
	    => await ExecuteAsync(() => _friendsAppService.GetAllFriendshipsAsync(GetCurrentUserId()));
    
    [HttpGet("get-all-friend-requests")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FriendRequestDto>))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllFriendRequests()
	    => await ExecuteAsync(() => _friendsAppService.GetAllFriendRequestsAsync(GetCurrentUserId()));
    
    [HttpPost("send-request")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SendFriendRequest(CreateFriendRequestDto dto)
        => await ExecuteWithoutContentAsync(() => _friendsAppService.SendFriendRequestAsync(dto, GetCurrentUserId()));
    
    [HttpPut("resolve-request")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ResolveFriendRequest(ResolveFriendRequestDto dto)
	    => await ExecuteWithoutContentAsync(() => _friendsAppService.ResolveFriendRequestAsync(dto, GetCurrentUserId()));
    
    [HttpDelete("remove-friend")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RemoveFriend(FriendshipDto dto)
	    => await ExecuteWithoutContentAsync(() => _friendsAppService.RemoveFriendAsync(dto, GetCurrentUserId()));
}