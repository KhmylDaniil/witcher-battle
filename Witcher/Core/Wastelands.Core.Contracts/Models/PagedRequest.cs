using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Models
{
	/// <summary>
	/// Модель запроса на получение списка с сортировкой и пагинацией.
	/// </summary>
	public sealed class PagedRequest : IOrderParams, IPagingParams
	{
		/// <summary>
		/// Наименование свойства для сортировки.
		/// </summary>
		public string OrderBy { get; set; } = "Id";

		/// <summary>
		/// Направление сортировки.
		/// </summary>
		public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;

		/// <summary>
		/// Количество записей на странице.
		/// </summary>
		public int PageSize { get; set; } = 10;

		/// <summary>
		/// Номер страницы.
		/// </summary>
		public int PageNumber { get; set; } = 1;
	}
}
