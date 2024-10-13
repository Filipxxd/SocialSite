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

    public async Task<Result<IEnumerable<GroupChatInfoDto>>> GetAllGroupChats(int currentUserId)
    {
        var groupChatsResult = await _chatService.GetAllGroupChatsAsync(currentUserId);

        if (!groupChatsResult.IsSuccess)
            return Result<IEnumerable<GroupChatInfoDto>>.Fail(groupChatsResult.Errors);

        var dtos = groupChatsResult.Entity.Adapt<IEnumerable<GroupChatInfoDto>>();

        return Result<IEnumerable<GroupChatInfoDto>>.Success(dtos);
    }

    public async Task<Result<GroupChatDto>> GetGroupChatByIdAsync(int groupChatId, int currentUserId)
    {
        var groupChatResult = await _chatService.GetGroupChatByIdAsync(groupChatId, currentUserId);

        if (!groupChatResult.IsSuccess)
            return Result<GroupChatDto>.Fail(groupChatResult.Errors);

        var dto = groupChatResult.Adapt<GroupChatDto>();

        return Result<GroupChatDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<DirectChatInfo>>> GetAllDirectChatsAsync(int currentUserId)
    {
        var directChatsResult = await _chatService.GetAllDirectChatsAsync(currentUserId);

        if (!directChatsResult.IsSuccess)
            return Result<IEnumerable<DirectChatInfo>>.Fail(directChatsResult.Errors);

        var dtos = directChatsResult.Entity.Adapt<IEnumerable<DirectChatInfo>>();

        return Result<IEnumerable<DirectChatInfo>>.Success(dtos);
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
