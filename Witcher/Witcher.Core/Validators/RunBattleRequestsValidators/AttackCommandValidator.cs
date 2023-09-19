using FluentValidation;
using System;
using Witcher.Core.Contracts.RunBattleRequests;

namespace Witcher.Core.Validators.RunBattleRequestsValidators
{
	public sealed class AttackCommandValidator : AbstractValidator<AttackCommand>
	{
		public AttackCommandValidator()
		{
			RuleFor(x => x.DamageValue).Must(NullOrPositive).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);
			RuleFor(x => x.AttackValue).Must(NullOrPositive).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);
			RuleFor(x => x.DefenseValue).Must(NullOrPositive).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);

			RuleFor(x => x.AttackType).IsInEnum();

			RuleFor(x => x.DefensiveSkill).Must(x => x is null || Enum.IsDefined(x.Value));
		}

		bool NullOrPositive(int? source) => !source.HasValue || source.Value >= 0;
	}
}
