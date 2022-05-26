using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Уведомление
	/// </summary>
	public class Notification: EntityBase
	{
		/// <summary>
		/// Конструктор пустой
		/// </summary>
		protected Notification()
		{
		}

		/// <summary>
		/// Конструктор уведомления
		/// </summary>
		/// <param name="name">Названия</param>
		/// <param name="message">Сообщение</param>
		/// <param name="duration">Длительность существования в минутах</param>
		public Notification(string name, string message, int duration)
		{
			Name = name;
			Message = message;
			Duration = duration;
			Receivers = new List<User>();
		}
		
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Длительность существования в минутах
		/// </summary>
		public int Duration { get; set; }

		#region navigation properties

		/// <summary>
		/// Получатели
		/// </summary>
		public List<User> Receivers { get; set; }

		#endregion navigation properties
	}
}
