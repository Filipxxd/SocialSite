using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SocialSite.Data.EF;

internal static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods()
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
                        && m.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

        var configurations = assembly
            .GetTypes()
            .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters)
            .SelectMany(c => c.GetInterfaces()
                .Where(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .Select(i => (ConfigType: c, EntityType: i.GenericTypeArguments.First())))
            .ToList();

        foreach (var (configType, entityType) in configurations)
        {
            var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(entityType);
            var configurationInstance = Activator.CreateInstance(configType);
            applyConcreteMethod.Invoke(modelBuilder, [configurationInstance]);
        }
    }
}
