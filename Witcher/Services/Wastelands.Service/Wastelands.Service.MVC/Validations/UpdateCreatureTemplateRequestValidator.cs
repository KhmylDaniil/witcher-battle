using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class UpdateCreatureTemplateRequestValidator : AbstractValidator<UpdateCreatureTemplateRequest>
	{
		public UpdateCreatureTemplateRequestValidator()
		{
			// Id намеренно не валидируется здесь — см. аналогичное примечание в UpdateCharacterRequestValidator.
			RuleFor(x => x.CreatureType).IsInEnum();

			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Description).MaximumLength(500).WithMessage(ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.HP).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Sta).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Int).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Ref).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Dex).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Body).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Emp).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Cra).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Will).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Speed).GreaterThanOrEqualTo(0).WithMessage(ExceptionMessages.ValueCantBeNegative);
			RuleFor(x => x.Luck).GreaterThanOrEqualTo(0).WithMessage(ExceptionMessages.ValueCantBeNegative);
		}
	}
}
