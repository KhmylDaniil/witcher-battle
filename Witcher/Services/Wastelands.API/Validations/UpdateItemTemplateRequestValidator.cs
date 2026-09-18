using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.API.Validations
{
	public class UpdateItemTemplateRequestValidator : AbstractValidator<UpdateItemTemplateRequest>
	{
		public UpdateItemTemplateRequestValidator()
		{
			RuleFor(x => x.ItemType).IsInEnum();

			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Description).MaximumLength(500).WithMessage(ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Weight).GreaterThanOrEqualTo(0).WithMessage(ExceptionMessages.ValueCantBeNegative);
			RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage(ExceptionMessages.ValueCantBeNegative);

			When(x => x.ItemType == ItemType.Weapon, () =>
			{
				RuleFor(x => x.AttackSkill).NotNull().WithMessage(ExceptionMessages.FieldCantBeEmpty);
				RuleFor(x => x.AttacksPerTurn).NotNull().GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
				RuleFor(x => x.DamageDiceCount).NotNull().GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
				RuleFor(x => x.DamageModifier).NotNull().WithMessage(ExceptionMessages.FieldCantBeEmpty);
				RuleFor(x => x.DamageType).NotNull().WithMessage(ExceptionMessages.FieldCantBeEmpty);
				RuleFor(x => x.WeaponKind).NotNull().WithMessage(ExceptionMessages.FieldCantBeEmpty);
				RuleFor(x => x.AttackRange).NotNull().GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
				RuleFor(x => x.HandsRequired).NotNull().InclusiveBetween(1, 2).WithMessage(ExceptionMessages.ValueMustBeBetween);
				RuleFor(x => x.Durability).NotNull().GreaterThan(0).WithMessage(ExceptionMessages.ValueMustBePositive);
			});
		}
	}
}
