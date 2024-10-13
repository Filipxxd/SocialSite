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

    public async Task<Result<IEnumerable<DirectMessageDto>>> GetAllPrivateMessages(int receivingUserId, User currentUser)
    {
        var messagesResult = await _messageService.GetAllDirectAsync(receivingUserId, currentUser.Id);

        if (!messagesResult.IsSuccess)
            return Result<IEnumerable<DirectMessageDto>>.Fail(messagesResult.Errors);

        var dtos = messagesResult.Entity.Adapt<IEnumerable<DirectMessageDto>>();

        return Result<IEnumerable<DirectMessageDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<GroupChatMessageDto>>> GetAllGroupChatMessagesAsync(int groupChatId, User currentUser)
    {
        var messagesResult = await _messageService.GetAllGroupChatAsync(groupChatId, currentUser.Id);

        if (!messagesResult.IsSuccess)
            return Result<IEnumerable<GroupChatMessageDto>>.Fail(messagesResult.Errors);

        var dtos = messagesResult.Adapt<IEnumerable<GroupChatMessageDto>>();

        return Result<IEnumerable<GroupChatMessageDto>>.Success(dtos);
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
