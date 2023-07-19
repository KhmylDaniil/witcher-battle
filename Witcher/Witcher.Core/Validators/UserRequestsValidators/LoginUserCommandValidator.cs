using FluentValidation;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.Core.Validators.UserRequestsValidators
{
	public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		public LoginUserCommandValidator()
		{
			RuleFor(x => x.Login).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Password).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);
		}
	}
}
