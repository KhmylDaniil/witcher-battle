using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Элемент ответа на запрос на получение списка способностей - защитный навык
	/// </summary>
	public class GetAbilityByIdResponseItemDefensiveSkill
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }


		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }
	}
}
