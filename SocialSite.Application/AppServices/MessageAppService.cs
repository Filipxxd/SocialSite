using SocialSite.Application.Dtos.Chats;
using SocialSite.Application.Dtos.Messages;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class MessageAppService
{
    private readonly IMessageService _messageService;

    public MessageAppService(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessageAsync(CreateMessageDto dto, int currentUserId)
    {
        var message = dto.Map(currentUserId);

        await _messageService.SendMessageAsync(message);
    }
    
    public async Task<ChatMessageDto> GetMessageByIdAsync(int messageId, int currentUserId)
	{
		var message = await _messageService.GetMessageByIdAsync(messageId, currentUserId);

		return message.Map(currentUserId);
	}
}
