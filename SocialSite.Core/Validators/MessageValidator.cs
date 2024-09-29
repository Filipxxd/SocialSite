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

        RuleFor(e => e.SenderId).NotEmpty();

        When(e => e.GroupChatId != null, () =>
        {
            RuleFor(e => e.ReceiverId).Null();
        })
        .Otherwise(() =>
        {
            RuleFor(e => e.ReceiverId).NotNull();
        });
    }
}
