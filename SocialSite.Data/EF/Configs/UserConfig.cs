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
        builder.HasIndex(e => e.Email).IsUnique();

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

        builder.HasOne(e => e.Settings)
            .WithOne(e => e.User)
            .HasForeignKey<UserSettings>(e => e.UserId);

        builder.HasMany(e => e.GroupUsers)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder.HasMany(e => e.SentFriendRequests)
            .WithOne(e => e.Sender)
            .HasForeignKey(e => e.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ReceivedFriendRequests)
            .WithOne(e => e.Receiver)
            .HasForeignKey(e => e.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Friendships)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
