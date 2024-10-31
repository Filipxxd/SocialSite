using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class ChatService : IChatService
{
    private readonly DataContext _context;

    public ChatService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Chat>> GetAllChatsAsync(int currentUserId)
    {
        return await _context.Chats
            .AsNoTracking()
            .Include(gc => gc.ChatUsers)
                .ThenInclude(cu => cu.User)
            .Where(gc => gc.ChatUsers.Any(uc => uc.UserId == currentUserId))
            .ToListAsync();
    }

    public async Task<Chat> GetChatByIdAsync(int chatId, int currentUserId)
    {
        return await _context.Chats
            .AsNoTracking()
            .Include(c => c.ChatUsers)
                .ThenInclude(uc => uc.User)
            .Include(c => c.Owner)
            .Include(c => c.Messages.OrderByDescending(m => m.SentAt))
                .ThenInclude(m => m.Sender)
            .Where(c => c.ChatUsers.Any(uc => uc.UserId == currentUserId))
            .SingleOrDefaultAsync(c => c.Id == chatId)
                ?? throw new NotFoundException("Chat was not found");
    }

    public async Task<Chat> CreateChatAsync(Chat chat, int currentUserId)
    {
        var userIds = chat.ChatUsers.Select(cu => cu.UserId).ToList();
        
        if (userIds.All(id => id != currentUserId))
            throw new NotValidException("Cannot create chats without current user participating in it");

        await ValidateUserIdsAsync(userIds);
        
        var allUsersAllowed = await AreUsersChatEligibleAsync(userIds, currentUserId);
        
        if (!allUsersAllowed)
            throw new NotValidException("One or more users are either not friend or have disabled non-friend messages.");

        if (chat.OwnerId is null)
        {
            var chatExists = await _context.Chats
                .AsNoTracking()
                .AnyAsync(c => c.OwnerId == null && c.ChatUsers.All(cu => userIds.Contains(cu.UserId)));

            if (chatExists)
                throw new NotValidException("Direct chat between users already exists");
        }

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return await _context.Chats
                    .AsNoTracking()
                    .Include(c => c.ChatUsers)
                        .ThenInclude(uc => uc.User)
                    .Include(c => c.Owner)
                    .SingleAsync(c => c.Id == chat.Id);
    }

    public async Task AssignUsersToGroupChatAsync(int groupChatId, IList<int> userIds, int currentUserId)
    {
        var groupChat = await _context.Chats
            .Include(gc => gc.ChatUsers)
            .SingleOrDefaultAsync(gc => gc.Id == groupChatId);

        if (groupChat is null)
            throw new NotValidException("Group chat was not found.");

        if (groupChat.OwnerId != currentUserId)            
            throw new NotValidException("Only the owner can modify users in the group chat.");

        await ValidateUserIdsAsync(userIds);
        
        var allUsersAllowed = await AreUsersChatEligibleAsync(userIds, currentUserId);

        if (!allUsersAllowed)
            throw new NotValidException("One or more users are either not friend or have disabled non-friend messages.");
        
        var currentGroupUserIds = groupChat.ChatUsers.Select(gu => gu.UserId).ToList();
        
        var usersToAdd = userIds.Except(currentGroupUserIds);
        
        foreach (var userId in usersToAdd)
            groupChat.ChatUsers.Add(new ChatUser { UserId = userId });

        var usersToRemove = groupChat.ChatUsers
            .Where(gu => !userIds.Contains(gu.UserId));

        foreach (var groupUser in usersToRemove)
            groupChat.ChatUsers.Remove(groupUser);

        await _context.SaveChangesAsync();
    }
    
    private async Task ValidateUserIdsAsync(IList<int> userIds)
    {
        var validUserCount = await _context.Users.CountAsync(u => userIds.Contains(u.Id));

        if (validUserCount != userIds.Count)
            throw new NotValidException("One or more user ids are not valid.");
    }

    private async Task<bool> AreUsersChatEligibleAsync(IList<int> userIds, int currentUserId)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.AllowNonFriendChatAdd || 
                        u.Friendships.Any(f => 
                            (f.UserId == currentUserId && userIds.Contains(f.FriendId)) || 
                            (f.FriendId == currentUserId && userIds.Contains(f.UserId))
                        ))
            .Where(u => userIds.Contains(u.Id))
            .CountAsync() == userIds.Count;
    }
}
