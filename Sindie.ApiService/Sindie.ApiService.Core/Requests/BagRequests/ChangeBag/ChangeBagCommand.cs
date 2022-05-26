using Sindie.ApiService.Core.Contracts.BagRequests.ChangeBag;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BagRequests.ChangeBag
{
	/// <summary>
	/// Команда изменения сумки
	/// </summary>
	public class ChangeBagCommand: ChangeBagRequest
	{
		/// <summary>
		/// Конструктор команды изменения сумки
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="instanceId">Айди экземпляра</param>
		/// <param name="id">Айди</param>
		/// <param name="bagItems">Список предметов в сумке</param>
		public ChangeBagCommand(
			Guid gameId,
			Guid instanceId,
			Guid id,
			List<ChangeBagRequestItem> bagItems)
		{
			GameId = gameId;
			InstanceId = instanceId;
			Id = id;
			BagItems = bagItems;
		}
	}
}
