using FluentValidation;
using Witcher.Core.Contracts.GameRequests;

namespace Witcher.Core.Validators.GameRequestsValidators
{
	public sealed class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
	{
		public CreateGameCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
