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

    public async Task SendFriendRequestAsync(FriendRequest request)
    {
        var senderExists = await _context.Users.AnyAsync(u => u.Id == request.SenderId);
        var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.ReceiverId);
        
        if (!senderExists || !receiverExists)
            throw new NotValidException("Sender or receiver does not exist.");
        
        request.DateCreated = _dateTimeProvider.GetDateTime();
        request.State = FriendRequestState.Sent;
        
        _context.FriendRequests.Add(request);
        await _context.SaveChangesAsync();
    }
}
