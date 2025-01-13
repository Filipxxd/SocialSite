using Bogus;
using Microsoft.EntityFrameworkCore;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF;

public sealed class TestDataSeeder
{
	private readonly DataContext _context;

	private List<Role> Roles { get; set; } = [];
	private List<User> Users { get; set; } = [];
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
		await DeleteDataAndResetIdentity();

		var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			Roles = GenerateRoles();
			await _context.Roles.AddRangeAsync(Roles);
			await SaveChangesIdentityInsertAsync(Tables.Roles);

			Users = GenerateUsers();
			await _context.Users.AddRangeAsync(Users);
			await SaveChangesIdentityInsertAsync(Tables.Users);

			var friendshipData = EstablishFriendships();
			FriendRequests = friendshipData.FriendRequests;
			Friendships = friendshipData.Friendships;
			await _context.FriendRequests.AddRangeAsync(FriendRequests);
			await SaveChangesIdentityInsertAsync(Tables.FriendRequests);

			await _context.Friendships.AddRangeAsync(Friendships);
			await SaveChangesIdentityInsertAsync(Tables.Friendships);

			Posts = GeneratePosts();
			await _context.Posts.AddRangeAsync(Posts);
			await SaveChangesIdentityInsertAsync(Tables.Posts);

			Comments = GenerateComments();
			await _context.Comments.AddRangeAsync(Comments);
			await SaveChangesIdentityInsertAsync(Tables.Comments);

			Reports = GenerateReports();
			await _context.Reports.AddRangeAsync(Reports);
			await SaveChangesIdentityInsertAsync(Tables.Reports);

			await transaction.CommitAsync();
		}
		catch
		{
			await transaction.RollbackAsync();
		}
	}

	private async Task DeleteDataAndResetIdentity()
	{
		var tableNames = new[]
		{
			Tables.Reports,
			Tables.Comments,
			Tables.Posts,
			Tables.Friendships,
			Tables.FriendRequests,
			"AspNetUserRoles",
			Tables.Users,
			Tables.Roles
		};

		foreach (var tableName in tableNames)
		{
			var deleteSql = $"DELETE FROM {tableName}";
			await _context.Database.ExecuteSqlRawAsync(deleteSql);

			var checkIdentSql = $@"
				IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{tableName}' AND last_value IS NOT NULL)
                DBCC CHECKIDENT ('{tableName}', RESEED, 0);";

			await _context.Database.ExecuteSqlRawAsync(checkIdentSql);
		}

		await _context.SaveChangesAsync();
	}

	private async Task SaveChangesIdentityInsertAsync(string tableName)
	{
		await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] ON;");
		await _context.SaveChangesAsync();
		await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF;");
	}

	private static List<Role> GenerateRoles() => [
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

	private List<User> GenerateUsers()
	{
		var currentUserId = 1;

		var userFaker = new Faker<User>()
			.RuleFor(u => u.Id, _ => currentUserId++)
			.RuleFor(u => u.UserName, f => f.Internet.UserName())
			.RuleFor(u => u.FirstName, f => f.Name.FirstName())
			.RuleFor(u => u.LastName, f => f.Name.LastName())
			.RuleFor(u => u.Bio, f => f.Lorem.Sentence(6))
			.RuleFor(u => u.IsBanned, _ => false)
			.RuleFor(u => u.AllowNonFriendChatAdd, _ => true)
			.RuleFor(u => u.FriendRequestSetting, _ => FriendRequestSetting.AnyOne)
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
			.RuleFor(u => u.AccessFailedCount, _ => 0)
			.RuleFor(u => u.UserRoles, f => [new()
			{
				RoleId = f.PickRandom(Roles).Id
			}]);

		var users = userFaker.Generate(20);

		foreach (var user in users.OrderBy(_ => Guid.NewGuid()).Take(2))
			user.FriendRequestSetting = FriendRequestSetting.NoOne;

		return users;
	}

	private (List<FriendRequest> FriendRequests, List<Friendship> Friendships) EstablishFriendships()
	{
		var friendRequests = new List<FriendRequest>();
		var friendships = new List<Friendship>();
		var requestId = 1;
		var friendshipId = 1;

		var onlyFriendRequests = new List<(User, User)>
		{
			(Users[0], Users[1]),
			(Users[4], Users[0]),
			(Users[5], Users[0]),
		};

		foreach (var (sender, receiver) in onlyFriendRequests)
		{
			if (sender.FriendRequestSetting == FriendRequestSetting.NoOne || receiver.FriendRequestSetting == FriendRequestSetting.NoOne)
				continue;

			var friendRequest = new FriendRequest
			{
				Id = requestId++,
				State = FriendRequestState.Sent,
				DateCreated = GenerateRandomDate(200),
				SenderId = sender.Id,
				ReceiverId = receiver.Id
			};
			friendRequests.Add(friendRequest);
		}

		var friendshipPairs = new List<(User, User)>
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

		foreach (var (sender, receiver) in friendshipPairs)
		{
			if (sender.FriendRequestSetting == FriendRequestSetting.NoOne || receiver.FriendRequestSetting == FriendRequestSetting.NoOne)
				continue;

			var friendRequest = new FriendRequest
			{
				Id = requestId++,
				State = FriendRequestState.Accepted,
				DateCreated = GenerateRandomDate(200),
				SenderId = sender.Id,
				ReceiverId = receiver.Id
			};
			friendRequests.Add(friendRequest);

			var friendship = new Friendship
			{
				Id = friendshipId++,
				DateCreated = friendRequest.DateCreated,
				UserInitiatedId = sender.Id,
				UserAcceptedId = receiver.Id
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
			.RuleFor(r => r.PostId, (f, r) => f.PickRandom(Posts.Where(p => p.UserId != r.UserId)).Id);

		return reportFaker.Generate(50);
	}

	private static DateTime GenerateRandomDate(int maxHours)
	{
		var randomHours = new Faker().Random.Int(1, maxHours);
		return DateTime.Now.AddHours(-randomHours);
	}
}
