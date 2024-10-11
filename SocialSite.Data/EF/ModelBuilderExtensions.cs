using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SocialSite.Data.EF;

internal static class ModelBuilderExtensions
{
    public static void ApplyConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
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

    public static void SetEnumConstraints(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    var enumType = property.ClrType;
                    var enumValues = Enum.GetNames(enumType).Select(name => $"'{name}'");

                    var checkConstraint = property.IsNullable
                        ? $"[{property.GetColumnName()}] IS NULL OR [{property.GetColumnName()}] IN ({string.Join(",", enumValues)})"
                        : $"[{property.GetColumnName()}] IN ({string.Join(",", enumValues)})";

                    builder.Entity(entityType.ClrType).ToTable(e => e.HasCheckConstraint($"CK_{entityType.GetTableName()}_{property.GetColumnName()}", checkConstraint));
                }
            }
        }
    }
}
