using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class ChatMappingExtensions
{
    public static ChatDto Map(this Chat input, int currentUserId) => new()
    {
        Id = input.Id,
        Name = input.Name ?? input.ChatUsers.Select(e => e.User).First(e => e!.Id != currentUserId)!.Fullname,
        IsDirect = input.OwnerId == null,
        Messages = input.Messages.Select(message => new ChatMessageDto
        {
            Id = message.Id,
            SenderFullname = message.Sender!.Fullname,
            Content = message.Content,
            IsSentByCurrentUser = message.SenderId == currentUserId,
            SentAt = message.SentAt,
        }),
        Users = input.ChatUsers.Select(chatUser => new ChatUserDto
        {
            Id = chatUser.User!.Id,
            Fullname = chatUser.User!.Fullname
        })
    };

    public static Chat Map(this CreateChatDto input, int currentUserId) => new()
    {
        Name = input.Name,
        OwnerId = input.IsDirect ? null : currentUserId,
        ChatUsers = input.UserIds.Select(userId => new ChatUser
        {
            UserId = userId,
        }).ToList()
    };

    public static IEnumerable<ChatInfoDto> Map(this IEnumerable<Chat> input) => input.Select(chat => new ChatInfoDto
    {
        Id = chat.Id,
        Name = chat.Name,
        IsDirect = chat.OwnerId is null
    });
}

