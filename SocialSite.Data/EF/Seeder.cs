using Microsoft.EntityFrameworkCore;
using SocialSite.Domain.Constants;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF;

internal static class Seeder
{
	public static void SeedData(this ModelBuilder builder)
	{
		builder.SeedRoles();
	}
	
	private static void SeedRoles(this ModelBuilder builder)
	{
		var userRole = new Role
		{
			Id = 1,
			Name = Roles.User,
			NormalizedName = Roles.User.ToUpper()
		};

		var moderatorRole = new Role
		{
			Id = 2,
			Name = Roles.Moderator,
			NormalizedName = Roles.Moderator.ToUpper()
		};
		
		var adminRole = new Role
		{
			Id = 3,
			Name = Roles.Admin,
			NormalizedName = Roles.Admin.ToUpper()
		};
	    
		builder.Entity<Role>().HasData(adminRole, moderatorRole, userRole);
	}
}