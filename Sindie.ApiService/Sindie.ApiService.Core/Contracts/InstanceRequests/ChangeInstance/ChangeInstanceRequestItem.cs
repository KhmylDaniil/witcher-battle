using System;

namespace Sindie.ApiService.Core.Contracts.InstanceRequests.ChangeInstance
{
	/// <summary>
	/// Элемент запроса на изменение инстанса
	/// </summary>
	public class ChangeInstanceRequestItem
	{
		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid? CreatureId { get; set; }
		
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
