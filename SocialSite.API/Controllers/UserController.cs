using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.API.Controllers.Base;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Users;

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