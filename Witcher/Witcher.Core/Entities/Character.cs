using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Персонаж
	/// </summary>
	public class Character : Creature
	{
		/// <summary>
		/// Поле для <see cref="_userGameActivated"/>
		/// </summary>
		public const string UserGameActivatedField = nameof(_userGameActivated);

		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;
		private UserGameCharacter _userGameActivated;

		protected Character() { }

		public Character(Game game, CreatureTemplate creatureTemplate, Battle battle, string name, string Description, UserGame userGame)
			: base(creatureTemplate, battle, name, Description)
		{
			Game = game;
			UserGameCharacters = new List<UserGameCharacter>
			{
				new UserGameCharacter(this, userGame)
			};
		}

		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди активировавшего пользователя
		/// </summary>
		public Guid? UserGameActivatedId { get; protected set; }

		/// <summary>
		/// Время активации персонажа
		/// </summary>
		public DateTime? ActivationTime { get; set; }

		#region navigation properties

		/// <summary>
		/// Активировавший персонажа пользователь игры
		/// </summary>
		public UserGameCharacter UserGameActivated
		{
			get => _userGameActivated;
			set
			{
				_userGameActivated = value;
				UserGameActivatedId = value?.Id;
			}
		}

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		public List<UserGameCharacter> UserGameCharacters { get; set; }

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<Character>(nameof(Game));
				GameId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
