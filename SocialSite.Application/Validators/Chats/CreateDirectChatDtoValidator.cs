using FluentValidation;
using SocialSite.Application.Dtos.Chats;

namespace SocialSite.Application.Validators.Chats;

public sealed class CreateDirectChatDtoValidator : AbstractValidator<CreateDirectChatDto>
{
    public CreateDirectChatDtoValidator()
    {
        RuleFor(e => e.RecipientUserId).NotEmpty();
    }
}
