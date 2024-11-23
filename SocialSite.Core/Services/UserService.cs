using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
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

    public async Task<User> GetProfileInfoAsync(int userId)
    {
	    var result = await (from user in _context.Users
			    join image in _context.Images
				    on new { user.Id, Type = EntityType.Profile } 
				    equals new { Id = image.EntityId, Type = image.Entity } into imageGroup
			    from profileImage in imageGroup.DefaultIfEmpty()
			    where user.Id == userId
			    select new 
			    {
				    User = user,
				    ProfileImage = profileImage
			    })
		    .AsNoTracking()
		    .SingleOrDefaultAsync();
	    
	    if (result is null)
		    throw new NotFoundException("User was not found.");

	    result.User.ProfileImage = result.ProfileImage;
	    return result.User;
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
    
    public async Task<int> LoginAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            throw new NotValidException("Invalid credentials.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            throw new NotValidException("Invalid credentials.");

        return user.Id;
    }

    public async Task RegisterAsync(User user, string password)
    {
        var userExists = await _userManager.FindByNameAsync(user.UserName);

        if (userExists != null)
            throw new NotValidException("User with given username already exists.");

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new NotValidException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
