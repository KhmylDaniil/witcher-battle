using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос на получение списка битв
	/// </summary>
	public sealed class GetBattleQuery : GetBaseQuery, IRequest<IEnumerable<GetBattleResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }
	}
}
