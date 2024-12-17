using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialSite.Data.EF.Extensions;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Image> Reports { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ChatUser> GroupUsers { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurations(Assembly.GetExecutingAssembly());
        builder.SetEnumConstraints();
        
        builder.SeedData();
    }
}
