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
