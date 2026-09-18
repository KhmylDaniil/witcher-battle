using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Маппинг Battle → BattleDto, общий для BattleService и IBattleCombatService — оба возвращают
	/// BattleDto из каждого мутирующего метода, и оба должны одинаково обогащать BattleAttackDto
	/// (имена участников, справочное значение навыка, доступные части тела/защитные навыки), когда
	/// в бою есть активная атака.
	/// </summary>
	public interface IBattleDtoMapper
	{
		Task<BattleDto> MapAsync(Battle battle);
	}
}
