using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Application.Validators.Chats;
using SocialSite.Core.Services;
using SocialSite.Core.Utilities;
using SocialSite.Data.EF;
using SocialSite.Domain.Constants;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.Reflection;
using System.Text;
using TokenHandler = SocialSite.Application.Utilities.TokenHandler;

namespace SocialSite.API;

internal static class ConfigExtensions
{
	private static readonly string DbConnectionString = "Default";

	public static IServiceCollection AddContextWithIdentity(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
	{
		services.AddDbContext<DataContext>(options =>
		{
			options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
			options.UseSqlServer(configuration.GetConnectionString(DbConnectionString));
			options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

			if (environment.IsDevelopment())
			{
				options.EnableSensitiveDataLogging();
				options.EnableDetailedErrors();
			}
		});

		services.AddIdentity<User, Role>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 2;
				options.Password.RequiredUniqueChars = 0;
			})
			.AddEntityFrameworkStores<DataContext>()
			.AddDefaultTokenProviders();

		return services;
	}

	public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.SaveToken = true;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidAudience = configuration["JWT:ValidAudience"],
				ValidIssuer = configuration["JWT:ValidIssuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AccessSecret"] ?? ""))
			};
		});

		services.AddAuthorizationBuilder()
				.AddPolicy(AuthPolicies.SuperUsers, policy => policy.RequireRole(Roles.Admin))
				.AddPolicy(AuthPolicies.ElevatedUsers, policy =>
				{
					policy.RequireAssertion(context =>
						context.User.IsInRole(Roles.Moderator) || context.User.IsInRole(Roles.Admin));
				})
				.AddPolicy(AuthPolicies.RegularUsers, policy =>
				{
					policy.RequireAssertion(context => context.User.IsInRole(Roles.User) || context.User.IsInRole(Roles.Moderator) || context.User.IsInRole(Roles.Admin));
				});

		return services;
	}

	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddScoped<TokenHandler>();
		services.AddScoped<IFileHandler, FileHandler>();

		services.Scan(scan =>
		{
			scan.FromAssembliesOf(typeof(UserService))
				.AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Service")))
				.AsImplementedInterfaces()
				.WithScopedLifetime();

			scan.FromAssembliesOf(typeof(UserAppService))
				.AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("AppService")))
				.AsSelf()
				.WithScopedLifetime();
		});

		services.AddScoped<Domain.Utilities.ILogger>(provider =>
				new DbLogger(provider.GetRequiredService<IDateTimeProvider>(), configuration.GetConnectionString("Default")));

		return services;
	}

	public static IMvcBuilder AddEndpointValidation(this IMvcBuilder builder)
	{
		builder.ConfigureApiBehaviorOptions(options =>
		 {
			 options.InvalidModelStateResponseFactory = context =>
			 {
				 var errors = context.ModelState
					 .Where(ms => ms.Value?.Errors.Any() == true)
					 .ToDictionary(
						 kvp => kvp.Key,
						 kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage) ?? []
					 );

				 return new BadRequestObjectResult(new
				 {
					 Success = false,
					 Errors = errors
				 });
			 };
		 });

		builder.Services.AddValidatorsFromAssemblyContaining<CreateDirectChatDtoValidator>();
		builder.Services.AddFluentValidationAutoValidation();

		return builder;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialSite API", Version = "v1" });

			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "Enter 'Bearer' followed by space and your JWT token"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					[]
				}
			});
		});

		return services;
	}

	public static IEndpointRouteBuilder MapHubs(this WebApplication app)
	{
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			var hubTypes = assembly.GetTypes()
				.Where(t => typeof(Hub).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

			foreach (var hub in hubTypes)
			{
				var routeAttribute = hub.GetCustomAttribute<RouteAttribute>();
				if (routeAttribute == null || string.IsNullOrWhiteSpace(routeAttribute.Template))
					continue;

				var areaAttribute = hub.GetCustomAttribute<AreaAttribute>();
				var areaPrefix = areaAttribute?.RouteValue ?? string.Empty;

				var route = routeAttribute.Template.Replace("[area]/", string.Empty);
				var fullRouteTemplate = string.IsNullOrWhiteSpace(areaPrefix) ? route : $"{areaPrefix}/{route}";

				var mapHubMethod = typeof(HubEndpointRouteBuilderExtensions)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.FirstOrDefault(m => m.Name == "MapHub" && m.IsGenericMethod);

				if (mapHubMethod != null)
				{
					var genericMethod = mapHubMethod.MakeGenericMethod(hub);
					genericMethod.Invoke(null, new object[] { app, fullRouteTemplate });
				}
			}
		}
		return app;
	}
}
