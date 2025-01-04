using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class RoleConfig : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.ToTable(Tables.Roles);

		builder.HasMany(e => e.UserRoles)
			.WithOne(e => e.Role)
			.HasForeignKey(ur => ur.RoleId);
	}
}