using FluentValidation;
using Witcher.Core.Contracts.AbilityRequests;

namespace Witcher.Core.Validators.AbilityRequestsValidators
{
	public sealed class CreateAbilityCommandValidator : AbstractValidator<CreateAbilityCommand>
	{
		public CreateAbilityCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.AttackSkill).IsInEnum();

			RuleFor(x => x.AttackDiceQuantity).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.AttackSpeed).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.DamageType).IsInEnum();
		}
	}
}
