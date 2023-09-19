using FluentValidation;
using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.Core.Validators.CreatureTemplateRequestsValidators
{
	public class CreateCreatureTemplateCommandValidator : AbstractValidator<CreateCreatureTemplateCommand>
	{
		public CreateCreatureTemplateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(BaseData.ExceptionMessages.FieldCantBeEmpty);

			RuleFor(x => x.Name).MaximumLength(20).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Description).MaximumLength(100).WithMessage(BaseData.ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.CreatureType).IsInEnum();

			RuleFor(x => x.Int).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Ref).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Dex).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Body).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Emp).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Cra).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Will).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.HP).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);
			RuleFor(x => x.Sta).GreaterThan(0).WithMessage(BaseData.ExceptionMessages.ValueMustBePositive);

			RuleFor(x => x.Speed).GreaterThanOrEqualTo(0).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);
			RuleFor(x => x.Luck).GreaterThanOrEqualTo(0).WithMessage(BaseData.ExceptionMessages.ValueCantBeNegative);
		}
	}
}
