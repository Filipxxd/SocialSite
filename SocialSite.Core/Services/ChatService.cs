using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
using SocialSite.Core.Exceptions;
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

    public async Task<Chat> CreateChatAsync(Chat chat)
    {
        var validationResult = _validator.Validate<ChatValidator, Chat>(chat);

        if (!validationResult.IsValid)
            throw new NotValidException("");

        var userIds = chat.ChatUsers.Select(cu => cu.UserId);

        var chatExists = await _context.Chats
            .AsNoTracking()
            .AnyAsync(c => c.OwnerId == null && c.ChatUsers.All(cu => userIds.Contains(cu.UserId)));

        if (chatExists)
            throw new NotValidException("Direct chat between users already exists");

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return await _context.Chats
                    .AsNoTracking()
                    .Include(c => c.ChatUsers)
                        .ThenInclude(uc => uc.User)
                    .Include(c => c.Owner)
                    .SingleAsync(c => c.Id == chat.Id);
    }

    public async Task<Result> AssignUsersToGroupChatAsync(int groupChatId, IEnumerable<int> userIds, int currentUserId)
    {
        var groupChat = await _context.Chats
            .Include(gc => gc.ChatUsers)
            .SingleOrDefaultAsync(gc => gc.Id == groupChatId);

        if (groupChat is null)
            return Result.Fail(ResultErrors.NotFound, "Group chat was not found.");

        if (groupChat.OwnerId != currentUserId)
            return Result.Fail(ResultErrors.NotAuthorized, "Only the owner can modify users in the group chat.");

        var currentGroupUserIds = groupChat.ChatUsers.Select(gu => gu.UserId).ToList();
        var usersToAdd = userIds.Except(currentGroupUserIds).ToList();
        var usersToRemove = currentGroupUserIds.Except(userIds).ToList();

        foreach (var userId in usersToAdd)
        {
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return Result.Fail(ResultErrors.NotFound, $"User with ID {userId} was not found.");

            groupChat.ChatUsers.Add(new()
            {
                UserId = userId
            });
        }

        foreach (var userId in usersToRemove)
        {
            var groupUser = groupChat.ChatUsers.FirstOrDefault(gu => gu.UserId == userId);

            if (groupUser != null)
            {
                groupChat.ChatUsers.Remove(groupUser);
            }
        }

        await _context.SaveChangesAsync();

        return Result.Success();
    }


}
