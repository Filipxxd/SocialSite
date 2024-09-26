namespace SocialSite.Domain.Utilities;

public interface IDateTimeProvider
{
    public DateTime GetDateTime();
    public DateOnly GetDateOnly();
}
