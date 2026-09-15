using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class CreateCharacterRequestValidator : AbstractValidator<CreateCharacterRequest>
	{
		public CreateCharacterRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(30).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
