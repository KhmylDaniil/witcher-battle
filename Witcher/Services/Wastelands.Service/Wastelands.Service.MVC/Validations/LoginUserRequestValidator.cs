using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
	{
		public LoginUserRequestValidator()
		{
			RuleFor(x => x.Login).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Login).MaximumLength(30).WithMessage(ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Password).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Password).MaximumLength(30).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
/*
 * public sealed class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
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
 */