using Mapster;
using SocialSite.Application.Dtos.Messages;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers.Messages;

public class MessageMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NewDirectMessageDto, Message>()
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.ReceiverId, src => src.ReceiverId);

        config.NewConfig<NewGroupChatMessageDto, Message>()
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.GroupChatId, src => src.GroupChatId);

        config.NewConfig<Message, DirectMessageDto>()
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.SenderId, src => src.SenderId)
            .Map(dest => dest.ReceiverId, src => src.ReceiverId)
            .Map(dest => dest.SentAt, src => src.SentAt);

        config.NewConfig<Message, GroupChatMessageDto>()
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.SenderId, src => src.SenderId)
            .Map(dest => dest.SentAt, src => src.SentAt);
    }
}
