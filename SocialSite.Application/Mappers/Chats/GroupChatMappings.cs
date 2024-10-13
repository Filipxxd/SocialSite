using Mapster;
using SocialSite.Application.Dtos.Chats;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers.Chats;


public sealed class GroupChatMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NewGroupChatDto, GroupChat>()
            .Map(dest => dest.Name, src => src.Name);

        config.NewConfig<GroupChat, GroupChatInfoDto>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.GroupChatId, src => src.Id);

        config.NewConfig<GroupChat, GroupChatDto>()
            .Map(dest => dest.GroupChatId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.OwnerId, src => src.OwnerId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Users, src => src.GroupUsers.Select(e => new GroupChatUserDto
            {
                UserId = e.UserId,
                FullName = e.User!.FullName
            }));
    }
}
