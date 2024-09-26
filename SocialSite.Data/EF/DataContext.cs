using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Base;
using SocialSite.Domain.Utilities;
using System.Reflection;

namespace SocialSite.Data.EF;

public class DataContext(DbContextOptions<DataContext> options, IServiceProvider serviceProvider) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<FriendPair> FriendPairs { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    var enumType = property.ClrType;
                    var enumValues = Enum.GetNames(enumType).Select(name => $"'{name}'");

                    var checkConstraint = property.IsNullable
                        ? $"[{property.GetColumnName()}] IS NULL OR [{property.GetColumnName()}] IN ({string.Join(",", enumValues)})"
                        : $"[{property.GetColumnName()}] IN ({string.Join(",", enumValues)})";

                    modelBuilder.Entity(entityType.ClrType).ToTable(e => e.HasCheckConstraint($"CK_{entityType.GetTableName()}_{property.GetColumnName()}", checkConstraint));
                }
            }
        }
    }

    public int SaveChanges(int currentUserId)
    {
        UpdateEntities(currentUserId);
        return base.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(int currentUserId, CancellationToken cancellationToken = default)
    {
        UpdateEntities(currentUserId);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateEntities(int currentUserId)
    {
        using var scope = serviceProvider.CreateScope();
        var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        var currentDateTime = dateTimeProvider.GetDateTime();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ISoftDeletable softDeletableEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    softDeletableEntity.IsActive = true;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeletableEntity.IsActive = false;

                    if (softDeletableEntity is ChangeTracking deletedChangeTrackingEntity)
                    {
                        deletedChangeTrackingEntity.UpdatedById = currentUserId;
                        deletedChangeTrackingEntity.DateUpdated = currentDateTime;
                    }
                }
            }

            if (entry.Entity is ChangeTracking changeTrackingEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    changeTrackingEntity.CreatedById = currentUserId;
                    changeTrackingEntity.DateCreated = currentDateTime;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    changeTrackingEntity.UpdatedById = currentUserId;
                    changeTrackingEntity.DateUpdated = currentDateTime;
                }
            }
        }
    }
}
