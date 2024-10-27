using FluentValidation;
using SocialSite.Application.Dtos.Account;

namespace SocialSite.Application.Validators.Account;

public sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    private const string CZECH_CHARS = @"^[a-záčďéěíňóřšťúůýžA-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ\s]+$";

    public RegisterDtoValidator()
    {
        RuleFor(e => e.UserName).Length(3, 20)
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("'Username' can only contain letters, numbers and underscore");
        RuleFor(e => e.FirstName).NotEmpty()
            .Matches(CZECH_CHARS).WithMessage("'Firstname' must contain only characters from czech alphabet");
        RuleFor(e => e.LastName).NotEmpty()
            .Matches(CZECH_CHARS).WithMessage("'Lastname' must contain only characters from czech alphabet");
        RuleFor(e => e.Password).Length(6, 30);
    }
}
