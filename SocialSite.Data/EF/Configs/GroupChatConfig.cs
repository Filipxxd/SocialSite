using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class GroupChatConfig : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.ToTable(Tables.GroupChats);

        builder.HasKey(e => e.GroupChatId);

        builder.HasIndex(e => e.Name);

        builder.HasOne(e => e.Owner)
            .WithMany()
            .HasForeignKey(e => e.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.GroupUsers)
            .WithOne(e => e.GroupChat)
            .HasForeignKey(e => e.GroupChatId);

        builder.HasMany(e => e.Messages)
            .WithOne(e => e.GroupChat)
            .HasForeignKey(e => e.GroupChatId);
    }
}
