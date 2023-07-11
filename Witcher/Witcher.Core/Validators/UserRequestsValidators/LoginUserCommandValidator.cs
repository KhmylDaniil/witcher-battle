using FluentValidation;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.Core.Validators.UserRequestsValidators
{
	public class LoginUserCommandValidator : CustomAbstractValidator<LoginUserCommand>
	{
		public LoginUserCommandValidator()
		{
			RuleFor(x => x.Login).NotEmpty().WithMessage(FieldCantBeEmpty);

			RuleFor(x => x.Password).NotEmpty().WithMessage(FieldCantBeEmpty);
		}
	}
}
