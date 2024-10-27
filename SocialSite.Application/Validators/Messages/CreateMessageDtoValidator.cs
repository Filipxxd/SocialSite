using FluentValidation;
using SocialSite.Application.Dtos.Messages;

namespace SocialSite.Application.Validators.Messages;

public sealed class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(e => e.ChatId).NotEmpty();
        RuleFor(e => e.Content).NotEmpty();
    }
}
