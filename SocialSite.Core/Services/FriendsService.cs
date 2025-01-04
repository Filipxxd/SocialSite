using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class FriendsService : IFriendsService
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
        return await _context.Friendships
            .AsNoTracking()
            .Include(e => e.UserInitiated)
            .Include(e => e.UserAccepted)
            .Where(e => e.UserInitiatedId == currentUserId || e.UserAcceptedId == currentUserId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<FriendRequest>> GetAllFriendsRequestsAsync(int currentUserId)
    {
        return await _context.FriendRequests
            .AsNoTracking()
            .Include(e => e.Sender)
            .Include(e => e.Receiver)
            .Where(e => e.ReceiverId == currentUserId && e.State == FriendRequestState.Sent)
            .ToListAsync();
    }
    
    public async Task SendFriendRequestAsync(FriendRequest request)
    {
        var senderExists = await UserExistsAsync(request.SenderId);
        var receiverExists = await UserExistsAsync(request.ReceiverId);

        if (!senderExists || !receiverExists)
            throw new NotFoundException("Sender or receiver does not exist.");
        
        var existingRequest = await FriendRequestExistsAsync(request.SenderId, request.ReceiverId);
        var existingFriendship = await FriendshipExistsAsync(request.SenderId, request.ReceiverId);

        if (existingRequest || existingFriendship)
            throw new NotValidException("A friend request or friendship already exists.");
        
        request.DateCreated = _dateTimeProvider.GetDateTime();
        request.State = FriendRequestState.Sent;

        _context.FriendRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeFriendRequestAsync(int receiverId, int currentUserId)
    {
        var friendRequest = await _context.FriendRequests
            .SingleOrDefaultAsync(fr => fr.ReceiverId == receiverId 
                                       && fr.SenderId == currentUserId 
                                       && fr.State == FriendRequestState.Sent);

        if (friendRequest is null)
            throw new NotFoundException("Friend request not found.");

        _context.FriendRequests.Remove(friendRequest);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptFriendRequestAsync(int requestId, int currentUserId)
    {
        var request = await _context.FriendRequests
            .Include(e => e.Receiver)
            .SingleOrDefaultAsync(e => e.Id == requestId && e.State == FriendRequestState.Sent);

        if (request is null)
            throw new NotFoundException("Friend request not found.");

        if (request.ReceiverId != currentUserId)
            throw new NotValidException("User is not the receiver of the request.");
        
        var existingFriendship = await FriendshipExistsAsync(request.SenderId, request.ReceiverId);
        if (existingFriendship)
            throw new NotValidException("A friendship already exists.");

        request.State = FriendRequestState.Accepted;

        var friendship = new Friendship
        {
            DateCreated = _dateTimeProvider.GetDateTime(),
            UserInitiatedId = request.SenderId,
            UserAcceptedId = request.ReceiverId
        };

        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeclineFriendRequestAsync(int requestId, int currentUserId)
    {
        var request = await _context.FriendRequests
            .Include(e => e.Receiver)
            .SingleOrDefaultAsync(e => e.Id == requestId && e.State == FriendRequestState.Sent);

        if (request is null)
            throw new NotFoundException("Friend request not found.");

        if (request.ReceiverId != currentUserId)
            throw new NotValidException("User is not the receiver of the request.");

        request.State = FriendRequestState.Rejected;
        await _context.SaveChangesAsync();
    }
    
    public async Task RemoveFriendAsync(int friendId, int currentUserId)
    {
        var friendship = await _context.Friendships
            .SingleOrDefaultAsync(fs =>
                (fs.UserInitiatedId == currentUserId && fs.UserAcceptedId == friendId) ||
                (fs.UserAcceptedId == currentUserId && fs.UserInitiatedId == friendId));

        if (friendship is null)
            throw new NotFoundException("Friendship not found.");

        _context.Friendships.Remove(friendship);
        await _context.SaveChangesAsync();
    }
    
    private async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }
    
    private async Task<bool> FriendshipExistsAsync(int userId1, int userId2)
    {
        return await _context.Friendships.AnyAsync(fs =>
            (fs.UserInitiatedId == userId1 && fs.UserAcceptedId == userId2) ||
            (fs.UserAcceptedId == userId1 && fs.UserInitiatedId == userId2));
    }
    
    private async Task<bool> FriendRequestExistsAsync(int senderId, int receiverId)
    {
        return await _context.FriendRequests.Where(fr => fr.State == FriendRequestState.Sent).AnyAsync(fr =>
            (fr.SenderId == senderId && fr.ReceiverId == receiverId) ||
            (fr.SenderId == receiverId && fr.ReceiverId == senderId));
    }
}
