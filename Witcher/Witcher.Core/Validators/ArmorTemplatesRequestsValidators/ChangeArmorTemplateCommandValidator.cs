using FluentValidation;
using Witcher.Core.Contracts.ArmorTemplateRequests;

namespace Witcher.Core.Validators.ArmorTemplatesRequestsValidators
{
	public sealed class ChangeArmorTemplateCommandValidator : AbstractValidator<ChangeArmorTemplateCommand>
	{
		public ChangeArmorTemplateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Weight).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Price).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Armor).GreaterThanOrEqualTo(0).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);

			RuleFor(x => x.EncumbranceValue).GreaterThanOrEqualTo(0).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);

			RuleFor(x => x.BodyPartTypes).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleForEach(x => x.BodyPartTypes).IsInEnum();
		}
	}
}
