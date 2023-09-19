using FluentValidation;
using Witcher.Core.Contracts.GameRequests;

namespace Witcher.Core.Validators.GameRequestsValidators
{
	public sealed class ChangeGameCommandValidator : AbstractValidator<ChangeGameCommand>
	{
		public ChangeGameCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
