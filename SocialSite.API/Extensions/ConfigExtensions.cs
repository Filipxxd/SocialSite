using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialSite.Application.AppServices;
using SocialSite.Application.Validators.Chats;
using SocialSite.Core.Services;
using SocialSite.Core.Utilities;
using SocialSite.Data.EF;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;
using System.Text;

namespace SocialSite.API.Extensions;

internal static class ConfigExtensions
{
    public static IServiceCollection AddContextWithIdentity(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                // TODO: Setup realistically
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
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? ""))
            };
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(typeof(AccountService))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            scan.FromAssembliesOf(typeof(AccountAppService))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("AppService")))
                .AsSelf()
                .WithScopedLifetime();
        });

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

        builder.Services.AddValidatorsFromAssemblyContaining<CreateChatDtoValidator>();
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
}
