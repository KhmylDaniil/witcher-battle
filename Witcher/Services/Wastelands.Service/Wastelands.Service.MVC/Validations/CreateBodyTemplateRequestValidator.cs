using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class CreateBodyTemplateRequestValidator : AbstractValidator<CreateBodyTemplateRequest>
	{
		public CreateBodyTemplateRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Description).MaximumLength(500).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
