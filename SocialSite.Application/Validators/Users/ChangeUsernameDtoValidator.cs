using FluentValidation;
using SocialSite.Application.Constants;
using SocialSite.Application.Dtos.Users;

namespace SocialSite.Application.Validators.Users;

public sealed class ChangeUsernameDtoValidator : AbstractValidator<ChangeUsernameDto>
{
	public ChangeUsernameDtoValidator()
	{        
		RuleFor(e => e.UserId)
			.NotEmpty();
		
		RuleFor(e => e.NewUsername)
			.NotEmpty()
			.Matches(ValidationConstants.AlphaNumericRegex).WithMessage("'Username' can only contain letters and numbers.");
	}
}