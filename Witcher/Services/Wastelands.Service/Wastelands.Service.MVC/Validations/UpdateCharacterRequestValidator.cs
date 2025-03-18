using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class UpdateCharacterRequestValidator : AbstractValidator<UpdateCharacterRequest>
	{
		public UpdateCharacterRequestValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Id).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(30).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
