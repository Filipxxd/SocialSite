using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(Tables.Users);

        builder.HasKey(e => e.UserId);

        builder.HasIndex(e => e.FirstName);
        builder.HasIndex(e => e.LastName);
        builder.HasIndex(e => e.Role);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.Email).IsUnique();
        //builder.HasIndex(e => e.GoogleId).IsUnique();

        builder.Property(e => e.Role)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v));

        builder.Property(e => e.FirstName)
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .HasMaxLength(254);

        builder.Property(e => e.GoogleId)
            .HasMaxLength(128);

        builder.HasMany(e => e.Chats)
            .WithOne(e => e.Owner)
            .HasForeignKey(e => e.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
