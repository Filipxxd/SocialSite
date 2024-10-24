using SocialSite.Application.Dtos.Messages;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

public static class MessageMappingExtensions
{
    public static Message Map(this CreateMessageDto input, int currentUserId) => new()
    {
        Content = input.Content,
        ChatId = input.ChatId,
        SenderId = currentUserId
    };
}

