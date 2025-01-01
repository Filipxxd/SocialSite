using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class ChatConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable(Tables.Chats);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(50);
        
        builder.HasOne(e => e.Owner)
            .WithMany()
            .HasForeignKey(e => e.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ChatUsers)
            .WithOne(e => e.Chat)
            .HasForeignKey(e => e.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Messages)
            .WithOne(e => e.Chat)
            .HasForeignKey(e => e.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
