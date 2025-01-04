using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Constants;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;

namespace SocialSite.Core.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;
    
    public UserService(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<User> GetUserProfileAsync(string username)
    {
	    var query = _context.Users
		    .AsNoTracking()
		    .Include(u => u.Posts)
		    .Include(u => u.FriendshipsInitiatedByUser)
		    .Include(u => u.FriendshipsAcceptedByUser)
		    .Include(u => u.ReceivedFriendRequests)
		    .Include(u => u.SentFriendRequests);

	    return await query.SingleOrDefaultAsync(u => u.UserName == username)
		    ?? throw new NotFoundException("User was not found.");
    }
	
    public async Task<IEnumerable<User>> GetUsersAsync(UserFilter filter)
    {
	    var query = GetFilteredUsersQuery(filter);

	    query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

		return await query.ToListAsync();
	}

	public async Task<PaginationInfo> GetUsersPaginationInfoAsync(UserFilter filter)
	{
		var query = GetFilteredUsersQuery(filter);
		var totalItems = await query.CountAsync();

		return new(totalItems, filter.PageSize);
	}
	
    public async Task<User> GetProfileInfoAsync(int userId)
    {
	    var user = await _context.Users.FindAsync(userId)
	               ?? throw new NotFoundException("User was not found.");
	    
	    return user;
    }

    public async Task<User> UpdateProfileInfoAsync(User user)
    {
        var currentUser = await _context.Users.FindAsync(user.Id)
                          ?? throw new NotFoundException("User was not found.");

        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Bio = user.Bio;
        currentUser.AllowNonFriendChatAdd = user.AllowNonFriendChatAdd;
        currentUser.FriendRequestSetting = user.FriendRequestSetting;

        await _context.SaveChangesAsync();

        return currentUser;
    }

    public async Task UpdateProfileImageAsync(string imagePath, int currentUserId)
    {
	    var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == currentUserId)
	               ?? throw new NotFoundException("User was not found.");

	    user.ProfilePicturePath = imagePath;
	    
		await _context.SaveChangesAsync();
	}
    
    public async Task UpdateUserRoleAsync(int userId, string newRole)
    {
	    if (Roles.AvailableRoles.All(r => r != newRole))
		    throw new NotValidException("Role is not available.");
	    
	    var user = await _userManager.FindByIdAsync(userId.ToString());
	    if (user is null)
		    throw new NotFoundException("User was not found.");

	    var currentRoles = await _userManager.GetRolesAsync(user);
	    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
	    
	    if (!removeResult.Succeeded)
	    {
		    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
		    throw new NotValidException($"Error removing user from roles: {errors}.");
	    }

	    var addResult = await _userManager.AddToRoleAsync(user, newRole.ToUpper());
	    if (!addResult.Succeeded)
	    {
		    var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
		    throw new NotValidException($"Error adding user to role '{newRole}': {errors}.");
	    }
    }
    
    private IQueryable<User> GetFilteredUsersQuery(UserFilter filter)
    {
	    var query = _context.Users
		    .Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
		    .Where(u => u.Id != filter.CurrentUserId)
		    .OrderBy(u => u.UserName)
		    .AsNoTracking();

	    if (!string.IsNullOrEmpty(filter.SearchTerm))
		    query = query.Where(u => (u.FirstName+ " " + u.LastName).Contains(filter.SearchTerm));
		
	    return query;
    }

    public async Task ToggleUserBanAsync(int userId, bool banned)
    {
	    var user = await _context.Users.FindAsync(userId)
	               ?? throw new NotFoundException("User was not found.");

	    user.IsBanned = banned;

	    await _context.SaveChangesAsync();
    }
}
