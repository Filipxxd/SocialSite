using Mapster;
using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers.Account;


public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterDto, User>()
            .Map(dest => dest.UserName, src => src.UserName)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.SecurityStamp, src => Guid.NewGuid());
    }
}

