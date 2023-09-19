using FluentValidation;
using Witcher.Core.Contracts.ArmorTemplateRequests;

namespace Witcher.Core.Validators.ArmorTemplatesRequestsValidators
{
	public sealed class ChangeDamageTypeModifierForArmorTemplateCommandValidator : AbstractValidator<ChangeDamageTypeModifierForArmorTemplateCommand>
	{
		public ChangeDamageTypeModifierForArmorTemplateCommandValidator()
		{
			RuleFor(x => x.DamageTypeModifier).IsInEnum();
			RuleFor(x => x.DamageType).IsInEnum();
		}
	}
}
