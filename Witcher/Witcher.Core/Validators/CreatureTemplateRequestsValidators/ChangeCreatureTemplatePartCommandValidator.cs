using FluentValidation;
using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.Core.Validators.CreatureTemplateRequestsValidators
{
	public sealed class ChangeCreatureTemplatePartCommandValidator : AbstractValidator<ChangeCreatureTemplatePartCommand>
	{
		public ChangeCreatureTemplatePartCommandValidator()
		{
			RuleFor(x => x.ArmorValue).GreaterThanOrEqualTo(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
		}
	}
}
