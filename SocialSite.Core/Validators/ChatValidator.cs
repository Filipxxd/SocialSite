using FluentValidation;
using SocialSite.Domain.Models;

namespace SocialSite.Core.Validators;

public sealed class ChatValidator : AbstractValidator<Chat>
{
    public ChatValidator()
    {
        RuleFor(e => e.Name)
            .MaximumLength(50);

        RuleFor(e => e.ChatUsers)
           .Must(e => e.Count >= 2);
    }
}
