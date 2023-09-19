using FluentValidation;
using Witcher.Core.Contracts.ItemRequests;

namespace Witcher.Core.Validators.ItemRequestsValidators
{
	public sealed class GiveItemToAnotherCharacterCommandValidator : AbstractValidator<GiveItemToAnotherCharacterCommand>
	{
		public GiveItemToAnotherCharacterCommandValidator()
		{
			RuleFor(x => x.Quantity).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
		}
	}
}
