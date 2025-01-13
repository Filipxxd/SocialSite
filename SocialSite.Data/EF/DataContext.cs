using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialSite.Data.EF.Extensions;
using SocialSite.Domain.Models;

namespace SocialSite.Data.EF;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<
	User, 
	Role,
	int,
	IdentityUserClaim<int>,
	UserRole,
	IdentityUserLogin<int>,
	IdentityRoleClaim<int>,
	IdentityUserToken<int>>(options)
{
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
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
