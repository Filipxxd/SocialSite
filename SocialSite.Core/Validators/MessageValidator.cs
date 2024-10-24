using FluentValidation;
using SocialSite.Domain.Models;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Validators;

public class MessageValidator : AbstractValidator<Message>
{
    public MessageValidator(IDateTimeProvider dateTimeProvider)
    {
        RuleFor(e => e.Content).NotEmpty();

        RuleFor(e => e.SentAt).Must(dt => dt <= dateTimeProvider.GetDateTime());
    }
}
