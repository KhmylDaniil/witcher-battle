using System;
using Witcher.Core.Exceptions.EntityExceptions;

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

		private Character _character;
		private UserGame _userGame;

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
		public UserGameCharacter(
			Character character,
			UserGame userGame)
		{
			_character = character;
			_userGame = userGame;
		}

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserGameId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value ?? throw new EntityNotIncludedException<UserGameCharacter>(nameof(Character));
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
				_userGame = value ?? throw new EntityNotIncludedException<UserGameCharacter>(nameof(UserGame));
				UserGameId = value.Id;
			}
		}

		/// <summary>
		/// Персонаж, активированный пользователем
		/// </summary>
		public Character ActivateCharacter { get; set; }

		#endregion navigation properties
	}
}
