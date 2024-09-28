using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal sealed class ChatConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable(Tables.Chats);

        builder.HasKey(e => e.ChatId);

        builder.HasIndex(e => e.Name);

        builder.HasMany(e => e.Users)
            .WithMany(e => e.Chats);

        builder.HasMany(e => e.Messages)
            .WithOne(e => e.Chat);
    }
}
