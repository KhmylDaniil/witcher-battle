using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById
{
	/// <summary>
	/// Ответ на запрос способности
	/// </summary>
	public class GetAbilityByIdResponse: GetAbilityResponseItem
	{
		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<UpdateAbilityCommandItemAppledCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Защитные навыки
		/// </summary>
		public List<GetAbilityByIdResponseItemDefensiveSkill> DefensiveSkills { get; set; }
	}
}
