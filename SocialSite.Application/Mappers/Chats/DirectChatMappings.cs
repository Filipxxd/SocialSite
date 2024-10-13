using Mapster;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers.Chats;

public sealed class DirectChatMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, DirectChatInfo>()
            .Map(dest => dest.Name, src => src.UserName)
            .Map(dest => dest.UserId, src => src.Id);
    }
}
