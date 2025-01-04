using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(Tables.RefreshTokens);

        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Token).IsUnique();

        builder.Property(e => e.Token).HasMaxLength(50);
        
        builder.HasOne(r => r.User)
	        .WithMany(u => u.RefreshTokens)
	        .HasForeignKey(r => r.UserId)
	        .OnDelete(DeleteBehavior.Restrict);
    }
}
