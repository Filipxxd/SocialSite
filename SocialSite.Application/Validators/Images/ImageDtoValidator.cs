using FluentValidation;
using SocialSite.Application.Dtos.Images;

namespace SocialSite.Application.Validators.Images;

public sealed class ImageDtoValidator : AbstractValidator<ImageDto>
{
	public ImageDtoValidator()
	{
		RuleFor(e => e.Name)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(e => e.Base64)
			.NotEmpty()
			.Must(ValidationHelper.BeValidBase64).WithMessage("ImageData must be a valid Base64 string.");;
	}
}