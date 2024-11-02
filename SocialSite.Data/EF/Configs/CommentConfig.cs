using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF.Configs;

internal class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(Tables.Comments);

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.DateCreated);
        
        builder.Property(e => e.Content).HasMaxLength(500);
        
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Post)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
