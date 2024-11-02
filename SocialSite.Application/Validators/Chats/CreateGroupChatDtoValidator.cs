using FluentValidation;
using SocialSite.Application.Dtos.Chats;

namespace SocialSite.Application.Validators.Chats;

public sealed class CreateGroupChatDtoValidator : AbstractValidator<CreateGroupChatDto>
{
    public CreateGroupChatDtoValidator()
    {
        RuleFor(e => e.Name).NotEmpty().MaximumLength(50);
    }
}
