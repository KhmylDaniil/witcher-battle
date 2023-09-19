using FluentValidation;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.Core.Validators.UserRequestsValidators
{
	public sealed class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
	{
		public RegisterUserCommandValidator(IAppDbContext appDbContext)
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Email).MaximumLength(50).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Login).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Login).MaximumLength(25).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Password).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Password).MaximumLength(25).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
