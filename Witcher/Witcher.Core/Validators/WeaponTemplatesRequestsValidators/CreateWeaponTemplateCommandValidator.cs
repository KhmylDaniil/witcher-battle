using FluentValidation;
using Witcher.Core.Contracts.WeaponTemplateRequests;

namespace Witcher.Core.Validators.WeaponTemplatesRequestsValidators
{
	public sealed class CreateWeaponTemplateCommandValidator : AbstractValidator<CreateWeaponTemplateCommand>
	{
		public CreateWeaponTemplateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Weight).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Price).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.AttackSkill).IsInEnum();

			RuleFor(x => x.AttackDiceQuantity).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Durability).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.DamageType).IsInEnum();

			RuleFor(x => x.Range).Must(x => x is null || x.Value > 1).WithMessage("Значение должно быть больше минимального.");
		}
	}
}
