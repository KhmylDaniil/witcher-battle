using FluentValidation;
using Witcher.Core.Contracts.ItemRequests;

namespace Witcher.Core.Validators.ItemRequestsValidators
{
	public sealed class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
	{
		public CreateItemCommandValidator()
		{
			RuleFor(x => x.Quantity).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
		}
	}
}
