using Mapster;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

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
        var groupChatsResult = await _chatService.GetAllGroupChatsAsync(currentUserId);
        var directChatsResult = await _chatService.GetAllDirectChatsAsync(currentUserId);

        var dtos = groupChatsResult.Entity.Adapt<IEnumerable<ChatInfoDto>>().ToList();

        dtos.AddRange(directChatsResult.Entity.Adapt<IEnumerable<ChatInfoDto>>());

        return dtos;
    }

    public async Task<Result<GroupChatDto>> GetGroupChatByIdAsync(int groupChatId, int currentUserId)
    {
        var groupChatResult = await _chatService.GetGroupChatByIdAsync(groupChatId, currentUserId);

        if (!groupChatResult.IsSuccess)
            return Result<GroupChatDto>.Fail(groupChatResult.Errors);

        var dto = groupChatResult.Adapt<GroupChatDto>();

        return Result<GroupChatDto>.Success(dto);
    }

    public async Task<Result> CreateGroupChatAsync(NewGroupChatDto dto, User currentUser)
    {
        var groupChat = dto.Adapt<GroupChat>();
        groupChat.OwnerId = currentUser.Id;

        return await _chatService.CreateGroupChatAsync(groupChat);
    }

    public async Task<Result> AddToGroupChatAsync(UserInGroupChat dto, int currentUserId)
    {
        return await _chatService.AddToGroupChatAsync(dto.GroupChatId, dto.UserId, currentUserId);
    }

    public async Task<Result> RemoveFromGroupChatAsync(UserInGroupChat dto, int currentUserId)
    {
        return await _chatService.RemoveFromGroupChatAsync(dto.GroupChatId, dto.UserId, currentUserId);
    }
}
