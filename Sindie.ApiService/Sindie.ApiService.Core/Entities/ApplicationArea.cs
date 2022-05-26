using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Область применения
	/// </summary>
	public class ApplicationArea : EntityBase
	{
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Деятельности
		/// </summary>
		public List<Activity> Activities { get; set; }

		#endregion navigation properties
	}
}
