using System;

namespace Sindie.ApiService.Core.Contracts.InstanceRequests.CreateInstance
{
	/// <summary>
	/// Элемент запроса на создание инстанса
	/// </summary>
	public class CreateInstanceRequestItem
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }
	}
}
