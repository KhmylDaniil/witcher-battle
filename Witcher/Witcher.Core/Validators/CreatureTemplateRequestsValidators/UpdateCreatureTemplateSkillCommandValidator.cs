using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.Core.Validators.CreatureTemplateRequestsValidators
{
	public sealed class UpdateCreatureTemplateSkillCommandValidator : AbstractValidator<UpdateCreatureTemplateSkillCommand>
	{
		public UpdateCreatureTemplateSkillCommandValidator()
		{
			RuleFor(x => x.Value).InclusiveBetween(1, BaseData.DiceValue.Value).WithMessage(BaseData.ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Skill).IsInEnum();
		}
	}
}
