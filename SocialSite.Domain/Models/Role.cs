using Microsoft.AspNetCore.Identity;

namespace SocialSite.Domain.Models;

public class Role : IdentityRole<int>
{
	public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}

public class UserRole : IdentityUserRole<int>
{
	public override int UserId { get; set; }
	public override int RoleId { get; set; }
	
	public virtual User User { get; set; } = default!;
	public virtual Role Role { get; set; } = default!;
}