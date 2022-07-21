using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Игра пользователя
	/// </summary>
	public class UserGame : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		/// <summary>
		/// Поле для <see cref="_user"/>
		/// </summary>
		public const string UserField = nameof(_user);

		/// <summary>
		/// Поле для <see cref="_interface"/>
		/// </summary>
		public const string InterfaceField = nameof(_interface);

		/// <summary>
		/// Поле для <see cref="_gameRole"/>
		/// </summary>
		public const string GameRoleField = nameof(_gameRole);

		private Game _game;
		private User _user;
		private Interface _interface;
		private GameRole _gameRole;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected UserGame()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="game">Игра</param>
		/// <param name="interface">Интерфейс</param>
		/// <param name="gameRole">Роль в игре</param>
		public UserGame(
			User user,
			Game game,
			Interface @interface,
			GameRole gameRole
			)
		{
			User = user;
			Game = game;
			Interface = @interface;
			GameRole = gameRole;
		}

		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; protected set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid InterfaceId { get; protected set; }

		/// <summary>
		/// Айди роли в игре
		/// </summary>
		public Guid GameRoleId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Пользователь
		/// </summary>
		public User User
		{
			get => _user;
			set
			{
				_user = value ?? throw new ApplicationException("Пустое поле Пользователь");
				UserId = value.Id;
			}
		}

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			set
			{
				_game = value ?? throw new ApplicationException("Пустое поле Игра");
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Интерфейс
		/// </summary>
		public Interface Interface
		{
			get => _interface;
			set
			{
				_interface = value ?? throw new ApplicationException("Пустое поле Интерфейс");
				InterfaceId = value.Id;
			}
		}

		/// <summary>
		/// Роль в игре
		/// </summary>
		public GameRole GameRole
		{
			get => _gameRole;
			set
			{
				_gameRole = value ?? throw new ApplicationException("Пустое поле Роль в игре");
				GameRoleId = value.Id;
			}
		}

		/// <summary>
		/// Экземпляры игры
		/// </summary>
		public List<Battle> Instances { get; set; }

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		public List<UserGameCharacter> UserGameCharacters { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="game">Игра</param>
		/// <param name="@interface">Интерфейс</param>
		/// <param name="gameRole">Роль в игре</param>
		/// <returns>-</returns>
		[Obsolete("Только для тестов")]
		public static UserGame CreateForTest(
			User user = default,
			Game game = default,
			Interface @interface = default,
			GameRole gameRole = default)
		=> new UserGame()
		{
			UserId = user?.Id ?? default,
			GameId = game?.Id ?? default,
			InterfaceId = @interface?.Id ?? default,
			GameRoleId = gameRole?.Id ?? default,
			_user = user,
			_game = game,
			_interface = @interface,
			_gameRole = gameRole,
		};
	}
}
