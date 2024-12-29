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

        builder.HasIndex(e => new { e.FirstName, e.LastName });
        builder.HasIndex(e => e.FriendRequestSetting);

        builder.Property(e => e.FirstName).HasMaxLength(50);
        builder.Property(e => e.LastName).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(254);
        builder.Property(e => e.Bio).HasMaxLength(500);

        builder.Property(e => e.FriendRequestSetting)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (FriendRequestSetting)Enum.Parse(typeof(FriendRequestSetting), v));

        builder.HasMany(e => e.UserChats)
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

        builder.HasMany(e => e.FriendshipsAcceptedByUser)
            .WithOne(e => e.UserAccepted)
            .HasForeignKey(e => e.UserAcceptedId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.FriendshipsInitiatedByUser)
	        .WithOne(e => e.UserInitiated)
	        .HasForeignKey(e => e.UserInitiatedId)
	        .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Posts)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Reports)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
