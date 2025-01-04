using FluentValidation;
using SocialSite.Application.Dtos.Users;
using SocialSite.Domain.Constants;

namespace SocialSite.Application.Validators.Users;

public sealed class ChangeUserRoleDtoValidator : AbstractValidator<ChangeUserRoleDto>
{
	public ChangeUserRoleDtoValidator()
	{
		RuleFor(e => e.UserId)
			.NotEmpty();
		
		RuleFor(e => e.Role)
			.NotEmpty()
			.Must(r => r.Equals(Roles.User) || r.Equals(Roles.Moderator))
				.WithMessage($"Role must be either '{Roles.User}' or '{Roles.Moderator}'");
	}
}