using FluentValidation;
using SocialSite.Domain.Models;

namespace SocialSite.Core.Validators;

public sealed class GroupChatValidator : AbstractValidator<GroupChat>
{
    public GroupChatValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.OwnerId)
            .NotEmpty();
    }
}
