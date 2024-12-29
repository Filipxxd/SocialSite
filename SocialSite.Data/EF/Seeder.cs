using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialSite.Domain.Constants;

namespace SocialSite.Data.EF;

internal static class Seeder
{
	public static void SeedData(this ModelBuilder builder)
	{
		builder.SeedRoles();
	}
	
	private static void SeedRoles(this ModelBuilder builder)
	{
		var userRole = new IdentityRole<int>
		{
			Id = 1,
			Name = Roles.User,
			NormalizedName = Roles.User.ToUpper()
		};

		var adminRole = new IdentityRole<int>
		{
			Id = 2,
			Name = Roles.Admin,
			NormalizedName = Roles.Admin.ToUpper()
		};
	    
		builder.Entity<IdentityRole<int>>().HasData(adminRole, userRole);
	}
}