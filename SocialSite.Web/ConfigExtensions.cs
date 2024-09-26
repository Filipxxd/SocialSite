using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using SocialSite.Core.Services;
using SocialSite.Core.Utilities;
using SocialSite.Core.Validators;
using SocialSite.Domain.Utilities;
using System.Security.Claims;

namespace SocialSite.Web;

internal static class ConfigExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(typeof(UserService))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            scan.FromAssembliesOf(typeof(EntityValidator))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Validator")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }

    public static IServiceCollection AddGoogleAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(opt =>
        {
            opt.AccessDeniedPath = Routes.Login;
        })
        .AddGoogle(options =>
        {
            options.ClientId = configuration["Authentication:Google:ClientId"] ?? "";
            options.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? "";
            options.Scope.Add("email");
            options.ClaimActions.MapJsonKey("hosted_domain", "hosted_domain");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            options.ClaimActions.MapJsonKey("picture", "picture");
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");

            options.CallbackPath = new PathString("/signin-google");

            var currentUserAppService = services.BuildServiceProvider().GetRequiredService<CurrentUserAppService>();

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = currentUserAppService.HandleOAuthUserAuthentication,
                OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect(Routes.Logout);
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    public static IEndpointRouteBuilder AddGoogleEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(Routes.Login, async (HttpContext context) =>
        {
            await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Routes.Home });
        });

        routeBuilder.MapGet(Routes.Logout, async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect(Routes.Login);
        });

        return routeBuilder;
    }
}
