namespace SocialSite.Application.Dtos.Account;

public sealed class TokenDto
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
