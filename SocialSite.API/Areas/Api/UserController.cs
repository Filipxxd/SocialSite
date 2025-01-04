using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Images;
using SocialSite.Application.Dtos.Users;
using SocialSite.Domain.Filters;

namespace SocialSite.API.Areas.Api;

[Area("api")]
[Route("[area]/users")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class UserController : ApiControllerBase
{
	private readonly UserAppService _userAppService;

	public UserController(UserAppService userAppService)
	{
		_userAppService = userAppService;
	}

	 [HttpGet("search")]
	 [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedDto<UserSearchDto>))]
	 [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	 public async Task<IActionResult> GetFilteredUsers([FromQuery] UserFilter filter) 
	 	=> await ExecuteAsync(() => _userAppService.GetFilteredUsersAsync(filter, GetCurrentUserId()));
	
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
	
	[HttpPatch("update-profile-picture")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> UpdateProfileImage(ImageDto dto) 
		=> await ExecuteWithoutContentAsync(() => _userAppService.UpdateProfileImageAsync(dto, GetCurrentUserId()));
	
	[HttpPatch("{userId:int}/role")]
	[Authorize(Policy = AuthPolicies.SuperUsers)]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> UpdateProfileImage(int userId, string role) 
		=> await ExecuteWithoutContentAsync(() => _userAppService.UpdateUserRoleAsync(role, userId));
	
	[HttpPatch("{userId:int}")]
	[Authorize(Policy = AuthPolicies.SuperUsers)]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
	[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
	public async Task<IActionResult> UpdateProfileImage(int userId, bool banned) 
		=> await ExecuteWithoutContentAsync(() => _userAppService.ToggleUserBanAsync(userId, banned));
}
