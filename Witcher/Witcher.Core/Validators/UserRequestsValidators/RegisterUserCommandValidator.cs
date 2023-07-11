using FluentValidation;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.Core.Validators.UserRequestsValidators
{
	public class RegisterUserCommandValidator: CustomAbstractValidator<RegisterUserCommand>
	{
		public RegisterUserCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(FieldCantBeEmpty);

			RuleFor(x => x.Login).NotEmpty().WithMessage(FieldCantBeEmpty);

			RuleFor(x => x.Password).NotEmpty().WithMessage(FieldCantBeEmpty);

			RuleFor(x => x.Password).MaximumLength(25).WithMessage(MaxPasswordLength);
		}
	}
}
