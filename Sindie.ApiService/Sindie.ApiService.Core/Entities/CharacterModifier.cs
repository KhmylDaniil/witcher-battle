using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Модификатор персонажа
	/// </summary>
	public class CharacterModifier: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		/// <summary>
		/// Поле для <see cref="_modifier"/>
		/// </summary>
		public const string ModifierField = nameof(_modifier);

		private Character _character;
		private Modifier _modifier;

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Айди модификатора
		/// </summary>
		public Guid ModifierId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value ?? throw new ApplicationException("Нужно передать персонажа");
				CharacterId = value.Id;
			}
		}

		/// <summary>
		/// Модификатор
		/// </summary>
		public Modifier Modifier
		{
			get => _modifier;
			set
			{
				_modifier = value ?? throw new ApplicationException("Нужно передать модификатор");
				ModifierId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
