using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal sealed class UserSettingsConfig : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable(Tables.UsersSettings);

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.FriendRequestSettingState)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (FriendRequestSettingState)Enum.Parse(typeof(FriendRequestSettingState), v));
    }
}

