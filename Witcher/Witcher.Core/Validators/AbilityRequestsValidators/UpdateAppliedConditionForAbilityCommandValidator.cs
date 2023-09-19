using FluentValidation;
using Witcher.Core.Contracts.AbilityRequests;

namespace Witcher.Core.Validators.AbilityRequestsValidators
{
	public sealed class UpdateAppliedConditionForAbilityCommandValidator : AbstractValidator<UpdateAppliedConditionForAbilityCommand>
	{
		public UpdateAppliedConditionForAbilityCommandValidator()
		{
			RuleFor(x => x.ApplyChance).InclusiveBetween(1, 100).WithMessage(BaseData.ExceptionMessages.ValueMustBeBetween);

			RuleFor(x => x.Condition).IsInEnum();
		}
	}
}
