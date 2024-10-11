using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Utilities;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetDateTime() => DateTime.Now;
    public DateOnly GetDateOnly() => DateOnly.FromDateTime(DateTime.Now);
}
