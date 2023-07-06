using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Entities;

namespace Witcher.Core.Notifications
{
	public class JoinGameRequestNotification : YesOrNoDecisionNotification
	{
		public Guid SenderId { get; set; }

		public Guid GameId { get; set; }

		public string GameName { get; set; }
		protected JoinGameRequestNotification() { }

		public JoinGameRequestNotification(User sender, User receiver, Game game, string message)
		{
			SenderId = sender.Id;
			GameId = game.Id;
			User = receiver;
			GameName = game.Name;

			string isMessageEmpty = string.IsNullOrEmpty(message) ? string.Empty : "Сообщение пользователя: ";

			Message = string.Format("Пользователь {0} желает присоединиться к игре {1}. {2}{3}", sender.Name, GameName, isMessageEmpty, message);
		}

		public override IValidatableCommand Accept()
			=> new CreateUserGameCommand { UserId = SenderId, RoleId = BaseData.GameRoles.PlayerRoleId };

		public override IValidatableCommand Decline()
			=> new CreateNotificationCommand { ReceiverId = SenderId, Message = $"Мастер игры не разрешил вам присоединиться к игре {GameName}." };

	}
}
