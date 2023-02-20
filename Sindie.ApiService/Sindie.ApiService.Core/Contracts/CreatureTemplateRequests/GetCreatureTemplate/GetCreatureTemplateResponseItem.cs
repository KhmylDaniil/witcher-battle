using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Элемент ответа на запрос списка шаблонов существа
	/// </summary>
	public sealed class GetCreatureTemplateResponseItem
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

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
		/// Тип шаблона существа
		/// </summary>
		public CreatureType CreatureType { get; set; }

		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string BodyTemplateName { get; set; }

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
	}
}
