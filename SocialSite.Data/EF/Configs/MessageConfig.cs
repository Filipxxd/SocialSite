using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(Tables.Messages);

        builder.HasKey(e => e.MessageId);

        builder.HasOne(e => e.Sender)
            .WithMany(e => e.SentMessages)
            .HasForeignKey(e => e.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Receiver)
            .WithMany(e => e.ReceivedMessages)
            .HasForeignKey(e => e.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.GroupChat)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

