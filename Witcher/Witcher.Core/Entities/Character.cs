using System;
using System.Collections.Generic;

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

		private UserGameCharacter _userGameActivated;

		/// <summary>
		/// Айди активировавшего пользователя
		/// </summary>
		public Guid? UserGameActivatedId { get; protected set; }

		/// <summary>
		/// Время активации экземпляра
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

		#endregion navigation properties
	}
}
