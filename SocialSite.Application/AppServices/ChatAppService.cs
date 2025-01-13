using SocialSite.Application.Dtos.Chats;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Services;

namespace SocialSite.Application.AppServices;

public sealed class ChatAppService
{
    private readonly IChatService _chatService;

    public ChatAppService(IChatService chatService)
    {
        _chatService = chatService;
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

    public async Task<ChatDto> CreateDirectChatAsync(CreateDirectChatDto dto, int currentUserId)
    {
        var chat = await _chatService.CreateChatAsync(dto.Map(currentUserId), currentUserId);

        return chat.Map(currentUserId);
    }

    public async Task<ChatDto> CreateGroupChatAsync(CreateGroupChatDto dto, int currentUserId)
    {
        var chat = await _chatService.CreateChatAsync(dto.Map(currentUserId), currentUserId);

        return chat.Map(currentUserId);
    }
    
    public async Task AssignUsersToGroupChatAsync(AssignGroupChatUsersDto dto, int currentUserId)
    {
        await _chatService.AssignUsersToGroupChatAsync(dto.GroupChatId, dto.UserIds.ToList(), currentUserId);
    }
    
    public Task<bool> IsUserInChatAsync(int chatId, int userId)
		=> _chatService.IsUserInChatAsync(chatId, userId);
}
