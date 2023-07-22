using FluentValidation;
using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.Core.Validators.CreatureTemplateRequestsValidators
{
	public sealed class ChangeDamageTypeModifierForCreatureTemplateCommandValidator : AbstractValidator<ChangeDamageTypeModifierForCreatureTemplateCommand>
	{
		public ChangeDamageTypeModifierForCreatureTemplateCommandValidator()
		{
			RuleFor(x => x.DamageTypeModifier).IsInEnum();
			RuleFor(x => x.DamageType).IsInEnum();
		}
	}
}
