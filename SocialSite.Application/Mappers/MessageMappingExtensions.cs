using SocialSite.Application.Dtos.Chats;
using SocialSite.Application.Dtos.Messages;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class MessageMappingExtensions
{
    public static Message Map(this CreateMessageDto input, int currentUserId) => new()
    {
        Content = input.Content,
        ChatId = input.ChatId,
        SenderId = currentUserId
    };

    public static IEnumerable<ChatMessageDto> Map(this IEnumerable<Message> input, int currentUserId) 
	    => input.Select(e => e.Map(currentUserId));
    
    public static ChatMessageDto Map(this Message input, int currentUserId) 
	    => new ChatMessageDto
	    {
		    Id = input.Id,
		    SenderFullname = input.Sender!.Fullname,
		    Content = input.Content,
		    IsSentByCurrentUser = input.SenderId == currentUserId,
		    SentAt = input.DateCreated,
	    };
}

