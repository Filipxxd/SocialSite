using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
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

    public async Task<Result> SendMessageAsync(Message message)
    {
        message.SentAt = _dateTimeProvider.GetDateTime();

        var validationResult = _validator.Validate<MessageValidator, Message>(message);

        if (!validationResult.IsValid)
            throw new NotValidException();

        var chat = await _context.Chats.AsNoTracking()
            .Include(e => e.ChatUsers)
            .SingleOrDefaultAsync(e => e.Id == message.ChatId);

        if (chat is null)
            throw new NotFoundException();

        if (!chat.ChatUsers.Any(e => e.UserId == message.SenderId))
            throw new NotValidException();

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
