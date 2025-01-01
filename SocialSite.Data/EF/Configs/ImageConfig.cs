using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal class ImageConfig : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(Tables.Images);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(128);
        builder.Property(e => e.Path).HasMaxLength(100);
        
        builder.HasOne(e => e.Post)
	        .WithMany(e => e.Images)
	        .HasForeignKey(e => e.PostId)
	        .OnDelete(DeleteBehavior.Restrict);
    }
}
