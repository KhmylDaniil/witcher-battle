using Witcher.Core.Abstractions;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос на получение списка битв
	/// </summary>
	public class GetBattleQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetBattleResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
