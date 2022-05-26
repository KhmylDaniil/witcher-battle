using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Уведомление об удалении предмета
	/// </summary>
	public class NotificationDeletedItem: Notification
	{
		/// <summary>
		/// Поле для <see cref="_bag"/>
		/// </summary>
		public const string BagField = nameof(_bag);

		private Bag _bag;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected NotificationDeletedItem()
		{
		}

		/// <summary>
		/// Конструктор уведомления об удалении предмета
		/// </summary>
		/// <param name="bag">Сумка</param>
		/// <param name="message">Сообщение</param>
		public NotificationDeletedItem(Bag bag, string message)
        {
			Name = $"Предметы удалены";
			Message = message;
			Bag = bag;
			Duration = 24*60;
			Receivers = new List<User>();
		}

		/// <summary>
		/// Айди сумки
		/// </summary>
		public Guid BagId { get; protected set; }

		#region navigation properties
		/// <summary>
		/// Сумка
		/// </summary>
		public Bag Bag
		{
			get => _bag;
			set
			{
				_bag = value ?? throw new ApplicationException("Необходимо передать сумку");
				BagId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
