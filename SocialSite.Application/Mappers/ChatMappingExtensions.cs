using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class ChatMappingExtensions
{
    public static ChatDto Map(this Chat input, int currentUserId) => new()
    {
        ChatId = input.Id,
        Name = input.Name ?? input.ChatUsers.Select(e => e.User).First(e => e!.Id != currentUserId)!.Fullname,
        IsDirect = input.IsDirect,
        Messages = input.Messages.Select(message => new ChatMessageDto
        {
            Id = message.Id,
            SenderFullname = message.Sender!.Fullname,
            Content = message.Content,
            IsSentByCurrentUser = message.SenderId == currentUserId,
            SentAt = message.DateCreated,
        }),
        Users = input.ChatUsers.Select(chatUser => new ChatUserDto
        {
            Id = chatUser.User!.Id,
            Fullname = chatUser.User!.Fullname
        })
    };

    public static Chat Map(this CreateDirectChatDto input, int currentUserId) => new()
    {
        Name = null,
        OwnerId = null,
        ChatUsers = [
            new()
        {
            UserId = input.RecipientUserId
        }, new()
        {
            UserId = currentUserId
        }]
    };

    public static Chat Map(this CreateGroupChatDto input, int currentUserId) => new()
    {
        Name = input.Name,
        OwnerId = currentUserId,
        ChatUsers = [..input.RecipientUserIds.Select(userId => new ChatUser
        {
            UserId = userId,
        }), new ChatUser
        {
            UserId = currentUserId,
        }]
    };

    public static IEnumerable<ChatInfoDto> Map(this IEnumerable<Chat> input, int currentUserId) => input.Select(chat => new ChatInfoDto
    {
        ChatId = chat.Id,
        Name = chat.Name ?? chat.ChatUsers.Select(e => e.User).First(e => e!.Id != currentUserId)!.Fullname,
        IsDirect = chat.OwnerId is null
    });
}

