namespace SocialSite.Application.Dtos.Account;

public sealed class LoginDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
