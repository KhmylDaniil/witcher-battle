using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Validations
{
	public class PaintBattleMapHexesRequestValidator : AbstractValidator<PaintBattleMapHexesRequest>
	{
		public PaintBattleMapHexesRequestValidator()
		{
			// Попадание координат в размеры конкретной карты проверяет BattleMap.PaintHexes — здесь карта
			// ещё не загружена, поэтому только верхняя граница на размер запроса.
			RuleFor(x => x.Hexes).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Hexes.Count).LessThanOrEqualTo(BattleMap.MaxDimension * BattleMap.MaxDimension).WithMessage(ExceptionMessages.ValueMustBeBetween);
			RuleForEach(x => x.Hexes).ChildRules(hex =>
			{
				hex.RuleFor(h => h.TerrainType).IsInEnum();
				hex.RuleFor(h => h.TerrainStyle).IsInEnum();
			});
		}
	}
}
