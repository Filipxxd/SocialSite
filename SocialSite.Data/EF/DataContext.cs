using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Base;
using SocialSite.Domain.Utilities;
using System.Reflection;

namespace SocialSite.Data.EF;

public class DataContext(DbContextOptions<DataContext> options, IServiceProvider serviceProvider) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Coments { get; set; }
    public DbSet<ChatUser> GroupUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurations(Assembly.GetExecutingAssembly());
        builder.SetEnumConstraints();
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
