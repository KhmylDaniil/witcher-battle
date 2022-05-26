using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Модификатор шаблона предмета
	/// </summary>
	public class ItemTemplateModifier : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_modifier"/>
		/// </summary>
		public const string ModifierField = nameof(_modifier);

		/// <summary>
		/// Поле для <see cref="_itemTemplate"/>
		/// </summary>
		public const string ItemTemplateField = nameof(_itemTemplate);

		private Modifier _modifier;
		private ItemTemplate _itemTemplate;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected ItemTemplateModifier()
		{
		}

		/// <summary>
		/// Конструктор класса Модификатор шаблона предмета
		/// </summary>
		/// <param name="modifier">Модификатор</param>
		/// <param name="itemTemplate">Шаблон предмета</param>
		public ItemTemplateModifier(
			Modifier modifier,
			ItemTemplate itemTemplate)
		{
			Modifier = modifier;
			ItemTemplate = itemTemplate;
		}

		/// <summary>
		/// Айди модификатора
		/// </summary>
		public Guid ModifierId { get; protected set; }

		/// <summary>
		/// Айди шаблона предмета
		/// </summary>
		public Guid ItemTemplateId { get; protected set; }

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
		/// Шаблон предмета
		/// </summary>
		public ItemTemplate ItemTemplate
		{
			get => _itemTemplate;
			protected set
			{
				_itemTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон предмета");
				ItemTemplateId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
