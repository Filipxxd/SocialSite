using FluentValidation;
using SocialSite.Application.Dtos.Comments;

namespace SocialSite.Application.Validators.Comments;

public sealed class CreateCommentDtoValidator: AbstractValidator<CreateCommentDto>
{
	public CreateCommentDtoValidator()
	{
		RuleFor(e => e.PostId).NotEmpty();
		RuleFor(e => e.Content).MaximumLength(256);
	}
}