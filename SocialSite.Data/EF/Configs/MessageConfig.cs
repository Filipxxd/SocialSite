using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(Tables.Messages);

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.DateCreated);

        builder.Property(e => e.Content).HasMaxLength(500);
        
        builder.HasOne(e => e.Sender)
            .WithMany()
            .HasForeignKey(e => e.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Chat)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

