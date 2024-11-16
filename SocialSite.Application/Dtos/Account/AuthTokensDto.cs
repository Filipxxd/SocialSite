namespace SocialSite.Application.Dtos.Account;

public sealed class AuthTokensDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
