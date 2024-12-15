using FluentValidation;
using SocialSite.Application.Dtos.Friends;

namespace SocialSite.Application.Validators.Friends;

public sealed class CreateFriendRequestDtoValidator : AbstractValidator<CreateFriendRequestDto>
{
    public CreateFriendRequestDtoValidator()
    {
        RuleFor(e => e.ReceiverId).NotEmpty();
    }
}