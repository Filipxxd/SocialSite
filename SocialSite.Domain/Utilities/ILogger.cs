namespace SocialSite.Domain.Utilities;

public interface ILogger
{
    Task Log(string exception, string message, string location, string? userId);
}
