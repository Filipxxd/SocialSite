using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(Tables.Posts);

        builder.HasKey(p => p.Id);
        
        builder.HasIndex(p => p.DateCreated);
        builder.HasIndex(p => p.Visibility);

        builder.Property(e => e.Content).HasMaxLength(500);
        
        builder.Property(e => e.Visibility)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (PostVisibility)Enum.Parse(typeof(PostVisibility), v));
        
        builder.HasOne(p => p.User)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Comments)
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
