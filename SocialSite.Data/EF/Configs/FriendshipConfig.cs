using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class FriendshipConfig : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.ToTable(Tables.Friendships);

        builder.HasKey(f => f.Id);

        builder.HasIndex(c => c.DateCreated);
        
        builder.HasOne(f => f.UserInitiated)
            .WithMany(f => f.FriendshipsInitiatedByUser)
            .HasForeignKey(f => f.UserInitiatedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.UserAccepted)
            .WithMany(f => f.FriendshipsAcceptedByUser)
            .HasForeignKey(f => f.UserAcceptedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
