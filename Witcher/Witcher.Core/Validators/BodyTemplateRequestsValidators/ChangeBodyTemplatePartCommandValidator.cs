using FluentValidation;
using Witcher.Core.Contracts.BodyTemplateRequests;

namespace Witcher.Core.Validators.BodyTemplateRequestsValidators
{
	public class ChangeBodyTemplatePartCommandValidator : AbstractValidator<ChangeBodyTemplatePartCommand>
	{
		public ChangeBodyTemplatePartCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.BodyPartType).IsInEnum();

			RuleFor(x => x.DamageModifier).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.HitPenalty).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.MinToHit).InclusiveBetween(0, BaseData.DiceValue.Value).WithMessage(BaseData.ExceptionMessages.ValueMustBeBetween);

			RuleFor(x => x.MaxToHit).InclusiveBetween(0, BaseData.DiceValue.Value).WithMessage(BaseData.ExceptionMessages.ValueMustBeBetween);

			RuleFor(x => x.MinToHit).LessThanOrEqualTo(x => x.MaxToHit).WithMessage(BaseData.ExceptionMessages.MinValueCantBeGreaterMaxValue);
		}
	}
}
