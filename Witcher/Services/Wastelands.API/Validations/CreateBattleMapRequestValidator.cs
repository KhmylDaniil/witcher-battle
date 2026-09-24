using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Validations
{
	public class CreateBattleMapRequestValidator : AbstractValidator<CreateBattleMapRequest>
	{
		public CreateBattleMapRequestValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Name).MaximumLength(50).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Description).MaximumLength(500).WithMessage(ExceptionMessages.MaxFieldLength);
			RuleFor(x => x.Columns).InclusiveBetween(BattleMap.MinDimension, BattleMap.MaxDimension).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.Rows).InclusiveBetween(BattleMap.MinDimension, BattleMap.MaxDimension).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleFor(x => x.TerrainStyle).IsInEnum();
		}
	}
}
