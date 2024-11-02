using System.ComponentModel;

namespace SocialSite.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name is null) 
            return value.ToString();

        var field = type.GetField(name);
        if (field is null) 
            return name;

        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute?.Description ?? name;
    }
}
