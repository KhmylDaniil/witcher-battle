using FluentValidation;
using Witcher.Core.Contracts.GameRequests;

namespace Witcher.Core.Validators.GameRequestsValidators
{
	public sealed class JoinGameRequestValidator : AbstractValidator<JoinGameRequest>
	{
		public JoinGameRequestValidator()
		{
			RuleFor(x => x.Message).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
