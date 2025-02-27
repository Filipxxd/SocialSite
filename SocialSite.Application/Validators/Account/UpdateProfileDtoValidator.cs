using FluentValidation;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Users;

namespace SocialSite.Application.Validators.Account;

public sealed class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
{
	public UpdateProfileDtoValidator()
	{
		RuleFor(e => e.Firstname).NotEmpty()
			.Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Firstname' must contain only characters from czech alphabet");
		RuleFor(e => e.Lastname).NotEmpty()
			.Matches(ValidationConstants.CzechAlphabetRegex).WithMessage("'Lastname' must contain only characters from czech alphabet");
		RuleFor(e => e.Bio).MaximumLength(500);
	}
}