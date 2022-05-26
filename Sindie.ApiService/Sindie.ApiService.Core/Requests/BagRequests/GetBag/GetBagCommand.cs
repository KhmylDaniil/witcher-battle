using Sindie.ApiService.Core.Contracts.BagRequests.GetBag;
using System;

namespace Sindie.ApiService.Core.Requests.BagRequests.GetBag
{
	/// <summary>
	/// Комманда обработки запроса на получение сумки и предметов в сумке
	/// </summary>
	public class GetBagCommand: GetBagQuery
	{
		/// <summary>
		/// Конструктор для комманды обработки запроса на получение сумки и предметов в сумке
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="instanceId">Айди экземпляра</param>
		/// <param name="bagId">Айди сумки</param>
		/// <param name="itemName">Фильтр по названию предмета</param>
		/// <param name="itemTemplateName">Фильтр по названию шаблона предмета</param>
		/// <param name="slotName">Фильтр по названию слота</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="pageNumber">Номер страниицы</param>
		/// <param name="orderBy">Сортировка по полю</param>
		/// <param name="isAscending">Сортировка по возрастанию</param>
		public GetBagCommand(
			Guid gameId,
			Guid instanceId,
			Guid bagId,
			string itemName,
			string itemTemplateName,
			string slotName,
			int pageSize,
			int pageNumber,
			string orderBy,
			bool isAscending)
		{
			GameId = gameId;
			InstanceId = instanceId;
			BagId = bagId;
			ItemName = itemName;
			ItemTemplateName = itemTemplateName;
			SlotName = slotName;
			PageSize = pageSize;
			PageNumber = pageNumber;
			OrderBy = orderBy;
			IsAscending = isAscending;
		}
	}
}
