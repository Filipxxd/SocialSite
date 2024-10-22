using Mapster;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers.Chats;

public sealed class ChatMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, ChatInfoDto>()
            .Map(dest => dest.Name, src => src.UserName)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.IsDirect, src => true);

        config.NewConfig<GroupChat, ChatInfoDto>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.IsDirect, src => false);
    }
}
