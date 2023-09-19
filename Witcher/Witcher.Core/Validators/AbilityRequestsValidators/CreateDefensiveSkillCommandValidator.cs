using FluentValidation;
using Witcher.Core.Contracts.AbilityRequests;

namespace Witcher.Core.Validators.AbilityRequestsValidators
{
	public class CreateDefensiveSkillCommandValidator : AbstractValidator<CreateDefensiveSkillCommand>
	{
		public CreateDefensiveSkillCommandValidator()
		{
			RuleFor(x => x.Skill).IsInEnum();
		}
	}
}
