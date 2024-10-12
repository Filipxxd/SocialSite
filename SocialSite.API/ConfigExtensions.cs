using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Services;
using SocialSite.Core.Utilities;
using SocialSite.Core.Validators;
using SocialSite.Data.EF;
using SocialSite.Domain.Utilities;

namespace SocialSite.API;

internal static class ConfigExtensions
{
    public static IServiceCollection AddDBConnection(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddDbContextFactory<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        }, ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(typeof(EntityValidator))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Validator")))
                .AsSelf()
                .WithScopedLifetime();

            scan.FromAssembliesOf(typeof(UserService))
                .AddClasses(classes => classes.Where(type => type.IsClass && type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }
}
