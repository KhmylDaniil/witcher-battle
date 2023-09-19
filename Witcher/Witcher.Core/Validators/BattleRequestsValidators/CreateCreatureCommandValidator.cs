using FluentValidation;
using Witcher.Core.Contracts.BattleRequests;

namespace Witcher.Core.Validators.BattleRequestsValidators
{
	public sealed class CreateCreatureCommandValidator : AbstractValidator<CreateCreatureCommand>
	{
		public CreateCreatureCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
