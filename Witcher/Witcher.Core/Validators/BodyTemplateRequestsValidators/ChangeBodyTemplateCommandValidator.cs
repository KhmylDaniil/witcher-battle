using FluentValidation;
using Witcher.Core.Contracts.BodyTemplateRequests;

namespace Witcher.Core.Validators.BodyTemplateRequestsValidators
{
	public class ChangeBodyTemplateCommandValidator : AbstractValidator<ChangeBodyTemplateCommand>
	{
		public ChangeBodyTemplateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);
		}
	}
}
