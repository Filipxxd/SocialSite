using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class FriendRequestConfig : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable(Tables.FriendRequests);

        builder.HasKey(f => f.Id);

        builder.HasOne(f => f.Sender)
            .WithMany(f => f.SentFriendRequests)
            .HasForeignKey(f => f.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Receiver)
            .WithMany(f => f.ReceivedFriendRequests)
            .HasForeignKey(f => f.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

