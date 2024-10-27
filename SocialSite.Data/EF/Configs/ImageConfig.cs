using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal class ImageConfig : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(Tables.Images);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.HasIndex(e => new { e.EntityId, e.Entity });

        builder.Property(e => e.Entity)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (EntityName)Enum.Parse(typeof(EntityName), v));
    }
}
