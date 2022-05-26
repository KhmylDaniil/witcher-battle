using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Модификатор шаблона персонажа
	/// </summary>
	public class CharacterTemplateModifier : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_modifier"/>
		/// </summary>
		public const string ModifierField = nameof(_modifier);

		/// <summary>
		/// Поле для <see cref="_characterTemplate"/>
		/// </summary>
		public const string CharacterTemplateField = nameof(_characterTemplate);

		private Modifier _modifier;
		private CharacterTemplate _characterTemplate;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected CharacterTemplateModifier()
		{
		}

		/// <summary>
		/// Конструктор для Модификатора шаблона персонажа
		/// </summary>
		/// <param name="modifier">Модификатор</param>
		/// <param name="characterTemplate">Шаблон персонажа</param>
		public CharacterTemplateModifier(
			Modifier modifier,
			CharacterTemplate characterTemplate)
		{
			Modifier = modifier;
			CharacterTemplate = characterTemplate;
		}

		/// <summary>
		/// Айди модификатора
		/// </summary>
		public Guid ModifierId { get; protected set; }

		/// <summary>
		/// Айди шаблона персонажа
		/// </summary>
		public Guid CharacterTemplateId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Модификатор
		/// </summary>
		public Modifier Modifier
		{
			get => _modifier;
			protected set
			{
				_modifier = value ?? throw new ApplicationException("Необходимо передать модификатор");
				ModifierId = value.Id;
			}
		}

		/// <summary>
		/// Шаблон персонажа
		/// </summary>
		public CharacterTemplate CharacterTemplate
		{
			get => _characterTemplate;
			protected set
			{
				_characterTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон пермонажа");
				CharacterTemplateId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
