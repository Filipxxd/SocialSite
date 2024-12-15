namespace SocialSite.Application.Utilities;

public sealed class JwtSetup
{
    public string ValidAudience { get; set; } = default!;
    public string ValidIssuer { get; set; } = default!;
    public string AccessSecret { get; set; } = default!;
    public string RefreshSecret { get; set; } = default!;
    public int AccessValidHours { get; set; }
    public int RefreshValidHours { get; set; }
    public int ExtendedRefreshValidHours { get; set; }
}
