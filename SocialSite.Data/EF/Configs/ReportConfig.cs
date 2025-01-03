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

        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.DateCreated);
        builder.HasIndex(r => r.State);
        builder.HasIndex(r => r.Type);
        
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

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Post)
            .WithMany(p => p.Reports)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
