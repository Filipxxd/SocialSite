using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Friends;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/friends")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class FriendsController : ApiControllerBase
{
	private readonly FriendsAppService _friendsAppService;

	public FriendsController(FriendsAppService friendsAppService)
	{
		_friendsAppService = friendsAppService;
	}

	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FriendshipDto>))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> GetAllFriendships()
		=> await ExecuteAsync(() => _friendsAppService.GetAllFriendshipsAsync(GetCurrentUserId()));

	[HttpDelete("{friendId:int}")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> RemoveFriend(int friendId)
		=> await ExecuteWithoutContentAsync(() => _friendsAppService.RemoveFriendAsync(friendId, GetCurrentUserId()));
	
	[HttpGet("requests")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FriendRequestDto>))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> GetAllFriendRequests()
		=> await ExecuteAsync(() => _friendsAppService.GetAllFriendRequestsAsync(GetCurrentUserId()));

	[HttpPost("request/{receiverId:int}")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	public async Task<IActionResult> SendFriendRequest(int receiverId)
		=> await ExecuteWithoutContentAsync(() => _friendsAppService.SendFriendRequestAsync(new()
		{  ReceiverId = receiverId, }, GetCurrentUserId()));
    
	[HttpPut("request")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> ResolveFriendRequest(ResolveFriendRequestDto dto)
		=> await ExecuteWithoutContentAsync(() => _friendsAppService.ResolveFriendRequestAsync(dto, GetCurrentUserId()));
	
	[HttpDelete("request/{receiverId:int}")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	public async Task<IActionResult> RevokeFriendRequest(int receiverId)
		=> await ExecuteWithoutContentAsync(() => _friendsAppService.RevokeFriendRequestAsync(receiverId, GetCurrentUserId()));
}