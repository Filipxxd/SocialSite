using System.Net;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Dtos.Users;
using SocialSite.Application.Utilities;

namespace SocialSite.API.Areas.User;

[Area("user")]
[Route("api/[area]/users")]
public sealed class UserController : ApiControllerBase
{
	private readonly UserAppService _userAppService;

	public UserController(UserAppService userAppService)
	{
		_userAppService = userAppService;
	}

	 [HttpGet("search")]
	 [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedData<UserSearchDto>))]
	 [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	 public async Task<IActionResult> GetFilteredUsers(string searchTerm) 
	 	=> await ExecuteAsync(() => _userAppService.GetFilteredUsersAsync(searchTerm, GetCurrentUserId()));
	
	 [HttpGet("profile/{username}")]
	 [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserProfileDto))]
	 [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	 [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	 public async Task<IActionResult> GetUserProfile(string username) 
		 => await ExecuteAsync(() => _userAppService.GetUserProfileAsync(username, GetCurrentUserId()));
	 
	[HttpGet("my-profile")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MyProfileDto))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> GetProfile() 
		=> await ExecuteAsync(() => _userAppService.GetProfileInfoAsync(GetCurrentUserId()));
    
	[HttpPut("update-profile")]
	[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MyProfileDto))]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto) 
		=> await ExecuteAsync(() => _userAppService.UpdateProfileInfoAsync(dto, GetCurrentUserId()));
}