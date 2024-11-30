using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Utilities;
using SocialSite.Domain.Filters;

namespace SocialSite.API.Controllers;

[Authorize]
[Route("user")]
public sealed class UserController : ApiControllerBase
{
	private readonly UserAppService _userAppService;

	public UserController(UserAppService userAppService)
	{
		_userAppService = userAppService;
	}

	[HttpGet("search-users")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedData<UserSearchDto>))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> GetFilteredUsers(UserFilter userFilter, PageFilter pageFilter) 
		=> await ExecuteAsync(() => _userAppService.GetFilteredUsersAsync(userFilter, pageFilter, GetCurrentUserId()));
	
	[HttpGet("get-profile")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserProfileDto))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> GetProfile() 
		=> await ExecuteAsync(() => _userAppService.GetProfileInfoAsync(GetCurrentUserId()));
    
	[HttpPut("update-profile")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserProfileDto))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto) 
		=> await ExecuteAsync(() => _userAppService.UpdateProfileInfoAsync(dto, GetCurrentUserId()));
}