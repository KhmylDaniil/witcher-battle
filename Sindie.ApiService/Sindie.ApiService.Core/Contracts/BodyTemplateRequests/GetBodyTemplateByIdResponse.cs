using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Ответ на запрос шаблона тела по айди
	/// </summary>
	public sealed class GetBodyTemplateByIdResponse
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Дата изменения
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Айди создавшего пользователя
		/// </summary>
		public Guid CreatedByUserId { get; set; }

		/// <summary>
		/// Айди изменившего пользователя
		/// </summary>
		public Guid ModifiedByUserId { get; set; }

		/// <summary>
		/// Список частей шаблона тела
		/// </summary>
		public List<GetBodyTemplateByIdResponseItem> GetBodyTemplateByIdResponseItems { get; set; }
	}
}
