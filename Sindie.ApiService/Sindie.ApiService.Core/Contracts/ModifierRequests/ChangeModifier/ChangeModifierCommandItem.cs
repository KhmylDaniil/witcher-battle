using System;

namespace Sindie.ApiService.Core.Contracts.ModifierRequests.ChangeModifier
{
	/// <summary>
	/// Подкласс команды для изменения модификатора скрипта
	/// </summary>
	public class ChangeModifierCommandItem
	{
		/// <summary>
		/// Айди скрипта модификатора
		/// </summary>
		public Guid Id { get; set; }
		
		/// <summary>
		/// Айди скрипта
		/// </summary>
		public Guid ScriptId { get; set; }

		/// <summary>
		/// Айди события
		/// </summary>
		public Guid? EventId { get; set; }

		/// <summary>
		/// Время активации
		/// </summary>
		public DateTime ActivationTime { get; set; }

		/// <summary>
		/// Период активности в минутах
		/// </summary>
		public int PeriodOfActivity { get; set; }

		/// <summary>
		/// Период неактивности в минутах
		/// </summary>
		public int PeriodOfInactivity { get; set; }

		/// <summary>
		/// Количество повторений
		/// </summary>
		public int NumberOfRepetitions { get; set; }
	}
}
