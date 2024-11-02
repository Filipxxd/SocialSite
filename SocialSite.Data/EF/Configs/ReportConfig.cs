using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Configs;

internal sealed class ReportConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable(Tables.Reports);

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.DateCreated);
        builder.HasIndex(p => p.State);
        builder.HasIndex(p => p.Type);
        
        builder.Property(e => e.Content).HasMaxLength(500);
        
        builder.Property(e => e.State)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (ReportState)Enum.Parse(typeof(ReportState), v));

        builder.Property(e => e.Type)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (ReportType)Enum.Parse(typeof(ReportType), v));

        builder.HasOne(p => p.User)
            .WithMany(p => p.Reports)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Post)
            .WithMany()
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
