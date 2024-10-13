using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
using SocialSite.Core.Validators;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class ChatService : IChatService
{
    private readonly DataContext _context;
    private readonly EntityValidator _validator;

    public ChatService(EntityValidator validator, DataContext context)
    {
        _validator = validator;
        _context = context;
    }

    public async Task<Result<IEnumerable<GroupChat>>> GetAllGroupChatsAsync(int currentUserId)
    {
        var groupChats = await _context.GroupChats
            .AsNoTracking()
            .Include(gc => gc.GroupUsers)
            .Where(gc => gc.GroupUsers.Any(gu => gu.UserId == currentUserId))
            .ToListAsync();

        return Result<IEnumerable<GroupChat>>.Success(groupChats);
    }

    public async Task<Result<GroupChat>> GetGroupChatByIdAsync(int groupChatId, int currentUserId)
    {
        var groupChat = await _context.GroupChats
            .AsNoTracking()
            .Include(gc => gc.GroupUsers)
                .ThenInclude(gu => gu.User)
            .Include(gc => gc.Owner)
            .SingleOrDefaultAsync(gc => gc.Id == groupChatId);

        if (groupChat is null)
            return Result<GroupChat>.Fail(ResultErrors.NotFound, "Group chat was not found.");

        if (!groupChat.GroupUsers.Any(e => e.UserId == currentUserId))
            return Result<GroupChat>.Fail(ResultErrors.NotFound, "User is not part of group chat.");

        return Result<GroupChat>.Success(groupChat);
    }

    public async Task<Result<IEnumerable<User>>> GetAllDirectChatsAsync(int currentUserId)
    {
        var users = await _context.Messages
            .AsNoTracking()
            .Where(m => (m.SenderId == currentUserId || m.ReceiverId == currentUserId) && m.GroupChatId == null)
            .Select(m => m.SenderId == currentUserId ? m.Receiver! : m.Sender!)
            .Distinct()
            .ToListAsync();

        return Result<IEnumerable<User>>.Success(users);
    }

    public async Task<Result> CreateGroupChatAsync(GroupChat chat)
    {
        var validationResult = _validator.Validate<GroupChatValidator, GroupChat>(chat);

        if (!validationResult.IsValid)
            return Result.Fail(ResultErrors.NotValid, validationResult.Errors.Select(e => e.ErrorMessage));

        chat.GroupUsers.Add(new()
        {
            UserId = chat.OwnerId
        });

        _context.GroupChats.Add(chat);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> AddToGroupChatAsync(int groupChatId, int userId, int currentUserId)
    {
        var groupChat = await _context.GroupChats
            .Include(gc => gc.GroupUsers)
            .SingleOrDefaultAsync(gc => gc.Id == groupChatId);

        if (groupChat is null)
            return Result.Fail(ResultErrors.NotFound, "Group chat was not found.");

        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return Result.Fail(ResultErrors.NotFound, "User was not found.");

        if (groupChat.OwnerId != currentUserId)
            return Result.Fail(ResultErrors.NotAuthorized, "Only owner can add user to a group chat.");

        if (groupChat.GroupUsers.Any(gu => gu.UserId == userId))
            return Result.Fail(ResultErrors.NotValid, "User is already part of group chat.");

        groupChat.GroupUsers.Add(new()
        {
            UserId = userId
        });

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RemoveFromGroupChatAsync(int groupChatId, int userId, int currentUserId)
    {
        var groupChat = await _context.GroupChats
            .Include(gc => gc.GroupUsers.Where(e => e.UserId == userId))
            .SingleOrDefaultAsync(gc => gc.Id == groupChatId);

        if (groupChat is null)
            return Result.Fail(ResultErrors.NotFound, "Group chat was not found.");

        if (groupChat.OwnerId != currentUserId)
            return Result.Fail(ResultErrors.NotAuthorized, "Only owner can remove user from a group chat.");

        var groupUser = groupChat.GroupUsers.FirstOrDefault(gu => gu.UserId == userId);

        if (groupUser is null)
            return Result.Fail(ResultErrors.NotValid, "User is not part of group chat.");

        groupChat.GroupUsers.Remove(groupUser);

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
