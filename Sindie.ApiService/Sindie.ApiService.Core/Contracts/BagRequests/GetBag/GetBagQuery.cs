using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GetBag
{
	/// <summary>
	/// Запрос на получение сумки со списком предметов в сумке
	/// </summary>
	public class GetBagQuery : IRequest<GetBagResponse>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди экземпляра
		/// </summary>
		public Guid InstanceId { get; set; }

		/// <summary>
		/// Айди сумки
		/// </summary>
		public Guid BagId { get; set; }

		/// <summary>
		/// Фильтр по названию предмета
		/// </summary>
		public string ItemName { get; set; }

		/// <summary>
		/// Фильтр по названию шаблона предмета
		/// </summary>
		public string ItemTemplateName { get; set; }

		/// <summary>
		/// Фильтр по названию слота
		/// </summary>
		public string SlotName { get; set; }

		/// <summary>
		/// Количество записей на одной странице 
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Номер страницы, с которой вывести записи
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		/// Сортировка по полю
		/// </summary>
		public string OrderBy { get; set; }

		/// <summary>
		/// Сортировка по возрастанию
		/// </summary>
		public bool IsAscending { get; set; }
	}
}
