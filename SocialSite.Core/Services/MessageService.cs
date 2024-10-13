using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Constants;
using SocialSite.Core.Validators;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class MessageService : IMessageService
{
    private readonly DataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly EntityValidator _validator;

    public MessageService(DataContext context, IDateTimeProvider dateTimeProvider, EntityValidator validator)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
    }

    public async Task<Result<IEnumerable<Message>>> GetAllDirectAsync(int receivingUserId, int currentUserId)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == receivingUserId) || (m.ReceiverId == currentUserId && m.SenderId == receivingUserId))
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        return Result<IEnumerable<Message>>.Success(messages);
    }

    public async Task<Result<IEnumerable<Message>>> GetAllGroupChatAsync(int groupChatId, int currentUserId)
    {
        var isInGroup = await _context.Messages
            .AsNoTracking()
            .Include(m => m.GroupChat)
                .ThenInclude(gc => gc!.GroupUsers)
            .AnyAsync(m => m.GroupChat!.GroupUsers.Any(gu => gu.UserId == currentUserId));

        if (!isInGroup)
            return Result<IEnumerable<Message>>.Fail(ResultErrors.NotValid, "User is not part of group.");

        var messages = await _context.Messages
            .AsNoTracking()
            .Where(e => e.GroupChatId == groupChatId)
            .OrderByDescending(e => e.SentAt)
            .ToListAsync();

        return Result<IEnumerable<Message>>.Success(messages);
    }

    public async Task<Result> SendMessageAsync(Message message)
    {
        message.SentAt = _dateTimeProvider.GetDateTime();

        var validationResult = _validator.Validate<MessageValidator, Message>(message);

        if (!validationResult.IsValid)
            return Result.Fail(ResultErrors.NotValid, validationResult.Errors.Select(e => e.ErrorMessage));

        if (message.IsDirect)
        {
            var receivingUser = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == message.ReceiverId);

            if (receivingUser is null)
                return Result.Fail(ResultErrors.NotFound, $"Receiving user with given id: '{message.ReceiverId}' was not found.");
        }
        else
        {
            var groupChat = await _context.GroupChats
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == message.GroupChatId);

            if (groupChat is null)
                return Result.Fail(ResultErrors.NotFound, $"Group chat with given id: '{message.GroupChatId}' was not found.");
        }

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
