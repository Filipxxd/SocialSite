using FluentValidation;
using SocialSite.Application.Dtos.Reports;

namespace SocialSite.Application.Validators.Reports;

public sealed class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
{
	public CreateReportDtoValidator()
	{
		RuleFor(x => x.PostId).NotEmpty();
		RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
		RuleFor(x => x.Type).IsInEnum();
	}
}