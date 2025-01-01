using FluentValidation;
using SocialSite.Application.Dtos.Posts;
using SocialSite.Application.Validators.Images;

namespace SocialSite.Application.Validators.Posts;

public sealed class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
	public CreatePostDtoValidator(ImageDtoValidator imageDtoValidator)
	{
		RuleFor(e => e.Content).MaximumLength(500);
		RuleFor(e => e.Images)
			.Must(x => x.Count() <= 5).WithMessage("Maximum 5 images allowed");
		RuleForEach(e => e.Images).SetValidator(imageDtoValidator);
	}
}