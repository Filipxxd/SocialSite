using SocialSite.Application.Dtos.Account;
using SocialSite.Domain.Models;

namespace SocialSite.Application.Mappers;

internal static class AccountMappingExtensions
{
	public static User Map(this RegisterDto input) => new()
	{
		UserName = input.UserName,
		FirstName = input.FirstName,
		LastName = input.LastName,
		SecurityStamp = Guid.NewGuid().ToString()
	};
}
