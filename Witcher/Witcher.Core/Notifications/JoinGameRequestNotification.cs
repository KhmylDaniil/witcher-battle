using System;
using Witcher.Core.Entities;

namespace Witcher.Core.Notifications
{
	public class JoinGameRequestNotification : Notification
	{
		public Guid SenderId { get; set; }

		public Guid GameId { get; set; }
		protected JoinGameRequestNotification() { }

		public JoinGameRequestNotification(User sender, User receiver, Game game, string message)
		{
			SenderId = sender.Id;
			GameId = game.Id;
			User = receiver;

			string isMessageEmpty = string.IsNullOrEmpty(message) ? "Без дополнительного сообщения." : "Сообщение пользователя: ";

			Message = string.Format("Пользователь {0} желает присоединиться к игре {1}. {2}{3}", sender.Name, game.Name, isMessageEmpty, message);
		}
	}
}
