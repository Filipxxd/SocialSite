using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public class FriendsService : IFriendsService
{
    private readonly DataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public FriendsService(DataContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IEnumerable<Friendship>> GetAllFriendshipsAsync(int currentUserId)
    {
	    return await _context.Friendships.AsNoTracking()
		    .Include(e => e.User)
		    .Include(e => e.Friend)
		    .Where(e => e.UserId == currentUserId || e.FriendId == currentUserId)
		    .ToListAsync();
    }
    
    public async Task<IEnumerable<FriendRequest>> GetAllFriendsRequestsAsync(int currentUserId)
    {
	    return await _context.FriendRequests.AsNoTracking()
		    .Include(e => e.Sender)
		    .Include(e => e.ReceiverId)
		    .Where(e => e.ReceiverId == currentUserId)
		    .ToListAsync();
    }
    
    public async Task SendFriendRequestAsync(FriendRequest request)
    {
        var senderExists = await _context.Users.AnyAsync(u => u.Id == request.SenderId);
        var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.ReceiverId);
        
        if (!senderExists || !receiverExists)
            throw new NotFoundException("Sender or receiver does not exist.");
        
        var existingRequest = await _context.FriendRequests.AnyAsync(fr =>
	        (fr.SenderId == request.SenderId && fr.ReceiverId == request.ReceiverId) ||
	        (fr.ReceiverId == request.SenderId && fr.SenderId == request.ReceiverId));

        var existingFriendship = await _context.Friendships.AnyAsync(fs =>
	        (fs.UserId == request.SenderId && fs.FriendId == request.ReceiverId) ||
	        (fs.FriendId == request.SenderId && fs.UserId == request.ReceiverId));

        if (existingRequest || existingFriendship)
	        throw new NotValidException("A friend request or friendship already exists.");
        
        request.DateCreated = _dateTimeProvider.GetDateTime();
        request.State = FriendRequestState.Sent;
        
        _context.FriendRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptFriendRequestAsync(int requestId, int currentUserId)
    {
		var request = await _context.FriendRequests
			.Include(e => e.Receiver)
			.Where(e => e.State == FriendRequestState.Sent)
			.SingleOrDefaultAsync(e => e.Id == requestId);
	    
		if (request is null)
			throw new NotFoundException("FriendRequest not found.");
		
		if (request.ReceiverId != currentUserId)
			throw new NotValidException("User is not receiver of request.");
		
		var existingFriendship = await _context.Friendships.AnyAsync(fs =>
			(fs.UserId == request.SenderId && fs.FriendId == request.ReceiverId) ||
			(fs.FriendId == request.SenderId && fs.UserId == request.ReceiverId));

		if (existingFriendship)
			throw new NotValidException("A friend request or friendship already exists.");
		
		request.State = FriendRequestState.Accepted;

		_context.Friendships.Add(new()
		{
			DateCreated = _dateTimeProvider.GetDateTime(),
			UserId = request.SenderId,
			FriendId = request.ReceiverId
		});
		await _context.SaveChangesAsync();
    }
    
    public async Task DeclineFriendRequestAsync(int requestId, int currentUserId)
    {
	    var request = await _context.FriendRequests
		    .Include(e => e.Receiver)
		    .Where(e => e.State == FriendRequestState.Sent)
		    .SingleOrDefaultAsync(e => e.Id == requestId);
	    
	    if (request is null)
		    throw new NotFoundException("FriendRequest not found.");
		
	    if (request.ReceiverId != currentUserId)
		    throw new NotValidException("User is not receiver of request.");
		
	    request.State = FriendRequestState.Rejected;
	    await _context.SaveChangesAsync();
    }
    
    public async Task RemoveFriendAsync(int friendId, int currentUserId)
	{
	    var friendship = await _context.Friendships
		    .SingleOrDefaultAsync(fs =>
			    (fs.UserId == currentUserId && fs.FriendId == friendId) ||
			    (fs.FriendId == currentUserId && fs.UserId == friendId));
	    
	    if (friendship is null)
		    throw new NotFoundException("Friendship not found.");
	    
	    _context.Friendships.Remove(friendship);
	    await _context.SaveChangesAsync();
	}
}
