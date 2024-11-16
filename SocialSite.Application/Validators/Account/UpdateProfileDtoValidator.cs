using FluentValidation;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Users;

namespace SocialSite.Application.Validators.Account;

public sealed class UpdateProfileDtoValidator: AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileDtoValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty()
            .Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Firstname' must contain only characters from czech alphabet");
        RuleFor(e => e.LastName).NotEmpty()
            .Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Lastname' must contain only characters from czech alphabet");
        RuleFor(e => e.Bio).MaximumLength(500)
            .Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Lastname' must contain only characters from czech alphabet");
    }
}