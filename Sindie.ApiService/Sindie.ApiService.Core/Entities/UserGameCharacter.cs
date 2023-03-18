using System;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Персонаж пользователя игры
	/// </summary>
	public class UserGameCharacter: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		/// <summary>
		/// Поле для <see cref="_userGame"/>
		/// </summary>
		public const string UserGameField = nameof(_userGame);

		/// <summary>
		/// Поле для <see cref="_interface"/>
		/// </summary>
		public const string InterfaceField = nameof(_interface);

		private Character _character;
		private UserGame _userGame;
		private Interface _interface;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected UserGameCharacter()
		{
		}

		/// <summary>
		/// Конструктор персонажа пользователя игры
		/// </summary>
		/// <param name="character">Персонаж</param>
		/// <param name="userGame">Пользователь игры</param>
		/// <param name="interface">Интерфейс</param>
		public UserGameCharacter(
			Character character,
			UserGame userGame,
			Interface @interface)
		{
			_character = character;
			_userGame = userGame;
			_interface = @interface;
		}

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserGameId { get; protected set; }

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid InterfaceId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value ?? throw new ApplicationException("Необходимо передать персонажа");
				CharacterId = value.Id;
			}
		}

		/// <summary>
		/// Пользователь игры
		/// </summary>
		public UserGame UserGame
		{
			get => _userGame;
			set
			{
				_userGame = value ?? throw new ApplicationException("Необходимо передать пользователя игры");
				UserGameId = value.Id;
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
				_interface = value ?? throw new ApplicationException("Необходимо передать интерфейс");
				InterfaceId = value.Id;
			}
		}

		/// <summary>
		/// Персонаж, активированный пользователем
		/// </summary>
		public Character ActivateCharacter { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="character">Персонаж</param>
		/// <param name="userGame">Пользователь игры</param>
		/// <param name="interface">Интерфейс</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static UserGameCharacter CreateForTest(
			Character character = default,
			UserGame userGame = default,
			Interface @interface = default)
		=> new UserGameCharacter()
		{
			Character = character,
			UserGame = userGame,
			Interface = @interface ?? Interface.CreateForTest()
		};
	}
}
