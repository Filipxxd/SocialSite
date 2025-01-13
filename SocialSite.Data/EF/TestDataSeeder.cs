using Bogus;
using Microsoft.EntityFrameworkCore;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF;

public class TestDataSeeder
{
    private readonly DataContext _context;
    
    private List<Role> Roles { get; set; } = [];
    private List<User> Users { get; set; } = [];
    private List<UserRole> AspNetUserRoles { get; set; } = [];
    private List<FriendRequest> FriendRequests { get; set; } = [];
    private List<Friendship> Friendships { get; set; } = [];
    private List<Post> Posts { get; set; } = [];
    private List<Comment> Comments { get; set; } = [];
    private List<Report> Reports { get; set; } = [];

    public TestDataSeeder(DataContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync()
    {
        if (await _context.Roles.AnyAsync() ||
            await _context.Users.AnyAsync() ||
            await _context.UserRoles.AnyAsync() ||
            await _context.FriendRequests.AnyAsync() ||
            await _context.Friendships.AnyAsync() ||
            await _context.Posts.AnyAsync() ||
            await _context.Comments.AnyAsync() ||
            await _context.Reports.AnyAsync())
            throw new InvalidOperationException("One or more tables are not empty."); // TODO: Remove data & reset identity

        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
	        await ExecuteWithIdentityInsertAsync(Tables.Roles, async () =>
	        {
		        Roles = GenerateRoles();
		        await _context.Roles.AddRangeAsync(Roles);
		        await _context.SaveChangesAsync();
	        });

	        await ExecuteWithIdentityInsertAsync(Tables.Users, async () =>
	        {
		        Users = GenerateUsers();
		        await _context.Users.AddRangeAsync(Users);
		        await _context.SaveChangesAsync();
	        });

	        await ExecuteWithIdentityInsertAsync(nameof(UserRole), async () =>
	        {
		        AspNetUserRoles = AssignRolesToUsers();
		        await _context.UserRoles.AddRangeAsync(AspNetUserRoles);
		        await _context.SaveChangesAsync();
	        });

	        await ExecuteWithIdentityInsertAsync(Tables.FriendRequests, async () =>
	        {
		        var friendshipData = EstablishFriendships();
		        FriendRequests = friendshipData.FriendRequests;
		        Friendships = friendshipData.Friendships;
		        await _context.FriendRequests.AddRangeAsync(FriendRequests);
		        await _context.SaveChangesAsync();
	        });
		       
	        await ExecuteWithIdentityInsertAsync(Tables.FriendRequests, async () =>
	        {
		        var friendshipData = EstablishFriendships();
		        FriendRequests = friendshipData.FriendRequests;
		        Friendships = friendshipData.Friendships;
		        await _context.FriendRequests.AddRangeAsync(FriendRequests);
		        await _context.Friendships.AddRangeAsync(Friendships);
		        await _context.SaveChangesAsync();
	        });

	        await ExecuteWithIdentityInsertAsync(Tables.Posts, async () =>
	        {
		        Posts = GeneratePosts();
		        await _context.Posts.AddRangeAsync(Posts);
		        await _context.SaveChangesAsync();
	        });
	        
	        await ExecuteWithIdentityInsertAsync(Tables.Comments, async () =>
	        {
		        Comments = GenerateComments();
		        await _context.Comments.AddRangeAsync(Comments);
		        await _context.SaveChangesAsync();
	        });
	        
	        await ExecuteWithIdentityInsertAsync(Tables.Reports, async () =>
	        {
		        Reports = GenerateReports();
		        await _context.Reports.AddRangeAsync(Reports);
		        await _context.SaveChangesAsync();
	        });
	        
	        await transaction.CommitAsync();
	        
            Console.WriteLine("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Data seeding failed: {ex.Message}");
        }
    }
    
    private async Task ExecuteWithIdentityInsertAsync(string tableName, Func<Task> operation)
    {
		await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] ON;");
	    await operation.Invoke();
	    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF;");
    } 
    
    private static List<Role> GenerateRoles()
    {
        return
        [
            new Role
            {
	            Id = 1,
	            Name = Domain.Constants.Roles.Admin,
	            NormalizedName = Domain.Constants.Roles.Admin.ToUpper(),
	            ConcurrencyStamp = Guid.NewGuid().ToString()
            },

            new Role
            {
	            Id = 2,
	            Name = Domain.Constants.Roles.Moderator,
	            NormalizedName = Domain.Constants.Roles.Moderator.ToUpper(),
	            ConcurrencyStamp = Guid.NewGuid().ToString()
            },

            new Role
            {
	            Id = 3,
	            Name = Domain.Constants.Roles.User,
	            NormalizedName = Domain.Constants.Roles.User.ToUpper(),
	            ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        ];
    }
    
    private static List<User> GenerateUsers()
    {
        var userFaker = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Bio, f => f.Lorem.Sentence(6))
            .RuleFor(u => u.IsBanned, _ => false)
            .RuleFor(u => u.AllowNonFriendChatAdd, _ => true)
            .RuleFor(u => u.FriendRequestSetting, f => f.PickRandom<FriendRequestSetting>())
            .RuleFor(u => u.ProfilePicturePath, _ => null)
            .RuleFor(u => u.NormalizedUserName, (_, u) => u.UserName.ToUpper())
            .RuleFor(u => u.EmailConfirmed, _ => true)
            .RuleFor(u => u.PasswordHash, _ => string.Empty)
            .RuleFor(u => u.SecurityStamp, _ => Guid.NewGuid().ToString())
            .RuleFor(u => u.ConcurrencyStamp, _ => Guid.NewGuid().ToString())
            .RuleFor(u => u.PhoneNumber, _ => null)
            .RuleFor(u => u.PhoneNumberConfirmed, _ => false)
            .RuleFor(u => u.TwoFactorEnabled, _ => false)
            .RuleFor(u => u.LockoutEnd, _ => null)
            .RuleFor(u => u.LockoutEnabled, _ => true)
            .RuleFor(u => u.AccessFailedCount, _ => 0);
        
        var users = userFaker.Generate(20);
        
        foreach (var user in users.OrderBy(_ => Guid.NewGuid()).Take(2)) 
	        user.FriendRequestSetting = FriendRequestSetting.NoOne;

        return users;
    }
    
    private List<UserRole> AssignRolesToUsers()
    {
        var assignmentFaker = new Faker<UserRole>()
            .RuleFor(ur => ur.RoleId, f => f.PickRandom(Roles).Id);

        return Users.Select(u => new UserRole
        {
            UserId = u.Id,
            RoleId = assignmentFaker.Generate().RoleId
        }).ToList();
    }
    
    private (List<FriendRequest> FriendRequests, List<Friendship> Friendships) EstablishFriendships()
    {
        var friendRequests = new List<FriendRequest>();
        var friendships = new List<Friendship>();
        var requestId = 1;
        var friendshipId = 1;
        
        var friendshipPairs = new List<(User Sender, User Receiver)>
        {
            (Users[0], Users[2]), 
            (Users[0], Users[3]), 
            (Users[1], Users[4]),
            (Users[1], Users[5]), 
            (Users[2], Users[6]),
            (Users[3], Users[7]),
            (Users[4], Users[8]),
            (Users[5], Users[9]), 
            (Users[6], Users[10]),
            (Users[7], Users[11]),
            (Users[8], Users[12]),
            (Users[9], Users[13]),
            (Users[10], Users[14]),
            (Users[11], Users[15]),
            (Users[12], Users[16]),
            (Users[13], Users[17]),
            (Users[14], Users[18]),
            (Users[15], Users[19]),
            (Users[16], Users[0]),
            (Users[17], Users[1]),
            (Users[18], Users[2]),
            (Users[19], Users[3])
        };

        foreach (var pair in friendshipPairs)
        {
            if (pair.Sender.FriendRequestSetting == FriendRequestSetting.NoOne || pair.Receiver.FriendRequestSetting == FriendRequestSetting.NoOne)
                continue;
            
            var friendRequest = new FriendRequest
            {
                Id = requestId++,
                State = FriendRequestState.Accepted,
                DateCreated = GenerateRandomDate(200),
                SenderId = pair.Sender.Id,
                ReceiverId = pair.Receiver.Id
            };
            friendRequests.Add(friendRequest);
            
            var friendship = new Friendship
            {
                Id = friendshipId++,
                DateCreated = friendRequest.DateCreated,
                UserInitiatedId = pair.Sender.Id,
                UserAcceptedId = pair.Receiver.Id
            };
            friendships.Add(friendship);
        }

        return (friendRequests, friendships);
    }
    
    private List<Post> GeneratePosts()
    {
	    var postId = 1;

        var postFaker = new Faker<Post>()
            .RuleFor(p => p.Id, _ => postId++)
            .RuleFor(p => p.Content, f => f.Lorem.Sentences(2))
            .RuleFor(p => p.DateCreated, _ => GenerateRandomDate(200))
            .RuleFor(p => p.Visibility, f => f.PickRandom(PostVisibility.Everyone, PostVisibility.FriendsOnly, PostVisibility.Private))
            .RuleFor(p => p.UserId, f => f.PickRandom(Users).Id);
        
        return postFaker.Generate(60);
    }
    
    private List<Comment> GenerateComments()
    {
        var commentId = 1;

        var commentFaker = new Faker<Comment>()
            .RuleFor(c => c.Id, _ => commentId++)
            .RuleFor(c => c.Content, f => f.Lorem.Sentences(1))
            .RuleFor(c => c.DateCreated, _ => GenerateRandomDate(200))
            .RuleFor(c => c.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(c => c.PostId, f => f.PickRandom(Posts).Id);
        
        return commentFaker.Generate(100);
    }
    
    private List<Report> GenerateReports()
    {
        var reportId = 1;

        var reportFaker = new Faker<Report>()
            .RuleFor(r => r.Id, _ => reportId++)
            .RuleFor(r => r.Content, f => f.Lorem.Sentences(1))
            .RuleFor(r => r.DateCreated, _ => GenerateRandomDate(200))
            .RuleFor(r => r.Type, f => f.PickRandom<ReportType>())
            .RuleFor(r => r.State, f => f.PickRandom<ReportState>())
            .RuleFor(r => r.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(r => r.PostId, f => f.PickRandom(Posts).Id);
        
        return reportFaker.Generate(50);
    }
    
    private static DateTime GenerateRandomDate(int maxHours)
    {
        var randomHours = new Faker().Random.Int(1, maxHours);
        return DateTime.Now.AddHours(-randomHours);
    }
}
