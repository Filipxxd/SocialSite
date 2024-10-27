using FluentValidation;
using SocialSite.Application.Dtos.Chats;

namespace SocialSite.Application.Validators.Chats;

public sealed class CreateChatDtoValidator : AbstractValidator<CreateChatDto>
{
    public CreateChatDtoValidator()
    {
        When(e => e.IsDirect, () =>
        {
            RuleFor(e => e.Name).Empty();
            RuleFor(e => e.UserIds).Must(e => e.Count() == 2).WithMessage("Direct chat must contain exactly 2 UserIds");
        }).Otherwise(() =>
        {
            RuleFor(e => e.Name).NotEmpty().MaximumLength(50);
            RuleFor(e => e.UserIds).NotEmpty();
        });
    }
}
