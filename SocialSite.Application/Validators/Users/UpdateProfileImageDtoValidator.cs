using FluentValidation;
using SocialSite.Application.Dtos.Users;

namespace SocialSite.Application.Validators.Users;

public class UpdateProfileImageDtoValidator : AbstractValidator<UpdateProfileImageDto>
{
	public UpdateProfileImageDtoValidator()
	{
		RuleFor(e => e.FileName)
			.NotEmpty().WithMessage("FileName must be provided.");

		RuleFor(e => e.ImageData)
			.NotEmpty().WithMessage("ImageData must be provided.");
	}
}
	