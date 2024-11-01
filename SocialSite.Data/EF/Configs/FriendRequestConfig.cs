using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal sealed class FriendRequestConfig : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable(Tables.FriendRequests);

        builder.HasKey(f => f.Id);

        builder.HasIndex(c => c.DateCreated);
        
        builder.Property(e => e.State)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (FriendRequestState)Enum.Parse(typeof(FriendRequestState), v));
        
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

