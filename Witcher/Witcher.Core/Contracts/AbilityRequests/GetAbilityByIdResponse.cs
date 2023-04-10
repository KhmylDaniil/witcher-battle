using System.Collections.Generic;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Ответ на запрос способности
	/// </summary>
	public class GetAbilityByIdResponse : GetAbilityResponseItem
	{
		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<UpdateAttackFormulaCommandItemAppledCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Защитные навыки
		/// </summary>
		public List<GetAbilityByIdResponseItemDefensiveSkill> DefensiveSkills { get; set; }
	}
}
