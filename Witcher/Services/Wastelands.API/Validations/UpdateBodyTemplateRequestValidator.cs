using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Validations
{
	public class UpdateBodyTemplateRequestValidator : AbstractValidator<UpdateBodyTemplateRequest>
	{
		public UpdateBodyTemplateRequestValidator()
		{
			// Id намеренно не валидируется здесь — см. аналогичное примечание в UpdateCharacterRequestValidator.
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Description).MaximumLength(500).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
