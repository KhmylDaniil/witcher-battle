using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class CreateBodyTemplatePartRequestValidator : AbstractValidator<CreateBodyTemplatePartRequest>
	{
		public CreateBodyTemplatePartRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.BodyPartType).IsInEnum();
			RuleFor(x => x.DamageModifier).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.HitPenalty).GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.MinToHit).InclusiveBetween(BodyTemplatePart.MinHitRange, BodyTemplatePart.MaxHitRange).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.MaxToHit).InclusiveBetween(BodyTemplatePart.MinHitRange, BodyTemplatePart.MaxHitRange).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.MaxToHit).GreaterThanOrEqualTo(x => x.MinToHit).WithMessage(ExceptionMessages.MinValueCantBeGreaterMaxValue);
		}
	}
}
