namespace SocialSite.Core.Utilities;

public sealed class JwtSetup
{
    public string ValidAudience { get; set; } = default!;
    public string ValidIssuer { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public int ValidHours { get; set; }
}
