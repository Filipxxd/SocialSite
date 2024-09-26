using FluentValidation;
using SocialSite.Domain.Models;

namespace SocialSite.Core.Validators;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);

        RuleFor(e => e.GoogleId)
            .NotEmpty()
            .MaximumLength(128);
    }
}
