using SocialSite.Application.Dtos.Chats;
using SocialSite.Application.Mappers;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Application.AppServices;

public sealed class ChatAppService
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public ChatAppService(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }

    public async Task<IEnumerable<ChatInfoDto>> GetAllChatsAsync(int currentUserId)
    {
        var chats = await _chatService.GetAllChatsAsync(currentUserId);

        return chats.Map(currentUserId);
    }

    public async Task<ChatDto> GetChatByIdAsync(int chatId, int currentUserId)
    {
        var chat = await _chatService.GetChatByIdAsync(chatId, currentUserId);

        return chat.Map(currentUserId);
    }

    public async Task<ChatDto> CreateChatAsync(CreateChatDto dto, int currentUserId)
    {
        if (!dto.UserIds.Any(id => id == currentUserId))
            throw new NotValidException("Cannot create chats without current user participating in it");

        var chat = await _chatService.CreateChatAsync(dto.Map(currentUserId));

        return chat.Map(currentUserId);
    }

    public async Task<Result> AssignUsersToGroupChatAsync(AssignGroupChatUsersDto dto, int currentUserId)
    {
        return await _chatService.AssignUsersToGroupChatAsync(dto.GroupChatId, dto.UserIds, currentUserId);
    }
}
