
using FluentValidation;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Account;

namespace SocialSite.Application.Validators.Account;

public sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(e => e.UserName).Length(3, 20)
            .Matches(ValidationConstants.AlphaNumericRegex).WithMessage("'Username' can only contain letters and numbers.");
        RuleFor(e => e.FirstName).NotEmpty()
            .Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Firstname' must contain only characters from czech alphabet");
        RuleFor(e => e.LastName).NotEmpty()
            .Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Lastname' must contain only characters from czech alphabet");
        RuleFor(e => e.Password).Length(6, 30)
            .Matches(ValidationConstants.PasswordRegex).WithMessage(
                "'Password' must be 6-30 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }
}
