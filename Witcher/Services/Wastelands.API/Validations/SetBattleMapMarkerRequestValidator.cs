using FluentValidation;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Validations
{
	public class SetBattleMapMarkerRequestValidator : AbstractValidator<SetBattleMapMarkerRequest>
	{
		public SetBattleMapMarkerRequestValidator()
		{
			// BattleMapId/Column/Row приходят из маршрута; попадание в карту проверяет BattleMap.SetMarker.
			RuleFor(x => x.Text).NotEmpty().WithMessage(ExceptionMessages.FieldCantBeEmpty);
			RuleFor(x => x.Text).MaximumLength(BattleMapHex.MaxMarkerTextLength).WithMessage(ExceptionMessages.MaxFieldLength);
		}
	}
}
