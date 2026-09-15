using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Validations
{
	public class UpdateCharacterRequestValidator : AbstractValidator<UpdateCharacterRequest>
	{
		public UpdateCharacterRequestValidator()
		{
			// Id намеренно не валидируется здесь: каноническое значение приходит из маршрута
			// (см. CharactersApiController.Update), контроллер проставляет его в request уже после
			// того, как отработает автоматическая FluentValidation — валидация значения из тела
			// запроса здесь была бы валидацией данных, которые всё равно отбрасываются.

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
