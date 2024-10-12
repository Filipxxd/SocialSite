using Mapster;
using SocialSite.Application.Dtos.Messages;
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

    public async Task<IEnumerable<DirectMessageDto>> GetAllPrivateMessages(int receivingUserId, User currentUser)
    {
        var messages = await _messageService.GetAllDirectAsync(receivingUserId, currentUser.Id);

        return messages.Adapt<IEnumerable<DirectMessageDto>>();
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetAllGroupChatMessagesAsync(int groupChatId, User currentUser)
    {
        var messages = await _messageService.GetAllGroupChatAsync(groupChatId, currentUser.Id);

        return messages.Adapt<IEnumerable<GroupChatMessageDto>>();
    }

    public async Task<Result> SendPrivateMessageAsync(NewDirectMessageDto dto, User currentUser)
    {
        var message = dto.Adapt<Message>();
        message.SenderId = currentUser.Id;

        return await _messageService.SendMessageAsync(message);
    }

    public async Task<Result> SendGroupChatMessageAsync(NewGroupChatMessageDto dto, User currentUser)
    {
        var message = dto.Adapt<Message>();
        message.SenderId = currentUser.Id;

        return await _messageService.SendMessageAsync(message);
    }
}
