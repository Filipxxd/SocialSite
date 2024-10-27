using FluentValidation;
using SocialSite.Application.Dtos.Account;

namespace SocialSite.Application.Validators.Account;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(e => e.UserName).NotEmpty();
        RuleFor(e => e.Password).NotEmpty();
    }
}
