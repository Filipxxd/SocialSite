using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Data.EF.Extensions;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class MessageService : IMessageService
{
    private readonly DataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MessageService(DataContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task SendMessageAsync(Message message)
    {
        var chat = await _context.Chats.AsNoTracking()
            .Include(e => e.ChatUsers)
            .SingleOrDefaultAsync(e => e.Id == message.ChatId);

        if (chat is null)
            throw new NotFoundException("Chat was not found");

        if (chat.ChatUsers.All(e => e.UserId != message.SenderId))
            throw new NotValidException("Sender is not part of Chat");

        message.DateCreated = _dateTimeProvider.GetDateTime();
        
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteMessageAsync(int messageId, int currentUserId)
	{
		var message = await _context.Messages
			.IncludeMessageImages()
			.SingleOrDefaultAsync(e => e.Id == messageId)
				?? throw new NotFoundException("Message was not found.");

		if (message.SenderId != currentUserId)
			throw new NotValidException("User is not owner of message.");
		
		_context.Images.RemoveRange(message.Images);
		_context.Messages.Remove(message);
		await _context.SaveChangesAsync();
	}
    
    public async Task<Message> GetMessageByIdAsync(int messageId, int currentUserId)
	{
		var message = await _context.Messages.AsNoTracking()
			.IncludeMessageImages()
			.Include(e => e.Chat)
				.ThenInclude(e => e!.ChatUsers)
			.SingleOrDefaultAsync(e => e.Id == messageId)
				?? throw new NotFoundException("Message was not found.");
		
		if (message.Chat!.ChatUsers.Any(e => e.UserId != currentUserId))
			throw new NotValidException("User is not part of Chat");

		return message;
	}
}
