using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Слот шаблона персонажа
	/// </summary>
	public class CharacterTemplateSlot : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_characterTemplate"/>
		/// </summary>
		public const string CharacterTemplateField = nameof(_characterTemplate);

		/// <summary>
		/// Поле для <see cref="_slot"/>
		/// </summary>
		public const string SlotField = nameof(_slot);

		private CharacterTemplate _characterTemplate;
		private Slot _slot;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected CharacterTemplateSlot()
		{
		}

		/// <summary>
		/// Конструктор класса Слот шаблона персонажа
		/// </summary>
		/// <param name="characterTemplate">Шаблон персонажа</param>
		/// <param name="slot">Слот</param>
		public CharacterTemplateSlot(
			CharacterTemplate characterTemplate,
			Slot slot)
		{
			CharacterTemplate = characterTemplate;
			Slot = slot;
		}

		/// <summary>
		/// Айди шаблона персонажа
		/// </summary>
		public Guid CharacterTemplateId { get; protected set; }

		/// <summary>
		/// Айди слота
		/// </summary>
		public Guid SlotId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Шаблон персонажа
		/// </summary>
		public CharacterTemplate CharacterTemplate
		{
			get => _characterTemplate;
			protected set
			{
				_characterTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон персонажа");
				CharacterTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Слот
		/// </summary>
		public Slot Slot
		{
			get => _slot;
			protected set
			{
				_slot = value ?? throw new ApplicationException("Необходимо передать слот");
				SlotId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
