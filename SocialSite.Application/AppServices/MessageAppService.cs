using SocialSite.Application.Dtos.Messages;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

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
}
