using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class CreateCharacterRequestValidator : AbstractValidator<CreateCharacterRequest>
	{
		public CreateCharacterRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(30).WithMessage(ExceptionMessages.MaxFieldLength);

			RuleFor(x => x.Int).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Str).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Rea).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Dex).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Cra).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Emp).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Wil).InclusiveBetween(Character.MinStat, Character.MaxStat).WithMessage(ExceptionMessages.ValueMustBeBetween);
		}
	}
}
