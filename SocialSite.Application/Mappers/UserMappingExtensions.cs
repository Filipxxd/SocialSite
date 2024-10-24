using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

public static class UserMappingExtensions
{
    public static User Map(this RegisterDto input) => new()
    {
        UserName = input.UserName,
        FirstName = input.FirstName,
        LastName = input.LastName,
        SecurityStamp = Guid.NewGuid().ToString()
    };
}
