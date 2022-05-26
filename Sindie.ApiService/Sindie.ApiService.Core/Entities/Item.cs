using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Предмет
	/// </summary>
	public class Item : Prerequisite
	{
		/// <summary>
		/// Поле для <see cref="_slot"/>
		/// </summary>
		public const string SlotField = nameof(_slot);

		/// <summary>
		/// Поле для <see cref="_itemTemplate"/>
		/// </summary>
		public const string ItemTemplateField = nameof(_itemTemplate);

		/// <summary>
		/// Поле для <see cref="_script"/>
		/// </summary>
		public const string ScriptField = nameof(_script);

		private Slot _slot;
		private ItemTemplate _itemTemplate;
		private Script _script;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected Item()
		{
		}

		/// <summary>
		/// Конструктор класса предмет
		/// </summary>
		/// <param name="slot">Слот</param>
		/// <param name="itemTemplate">Шаблон предмета</param>
		/// <param name="script">Скрипт</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Item(
			Slot slot,
			ItemTemplate itemTemplate,
			Script script,
			ImgFile imgFile,
			string name,
			string description)
			: base(
				  imgFile,
				  name,
				  description)
		{
			Slot = slot;
			ItemTemplate = itemTemplate;
			Script = script;
			InteractionItems = new List<InteractionItem>();
		}

		/// <summary>
		/// Айди слота
		/// </summary>
		public Guid SlotId { get; protected set; }

		/// <summary>
		/// Айди шаблона предмета
		/// </summary>
		public Guid ItemTemplateId { get; protected set; }

		/// <summary>
		/// Айди скрипта
		/// </summary>
		public Guid? ScriptId { get; protected set; }

		/// <summary>
		/// Вес
		/// </summary>
		public double Weight { get; protected set; }

		/// <summary>
		/// Максимальное количество в стаке
		/// </summary>
		public int MaxQuantity { get; protected set; }

		#region navigation properties

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
				MaxQuantity = value.MaxQuantity;
				Weight = value.Weight;
			}
		}

		/// <summary>
		/// Скрипт
		/// </summary>
		public Script Script
		{
			get => _script;
			set
			{
				_script = value;
				ScriptId = value?.Id;
			}
		}

		/// <summary>
		/// Предметы во взаимодействии
		/// </summary>
		public List<InteractionItem> InteractionItems { get; set; }

		/// <summary>
		/// Предметы в сумке
		/// </summary>
		public List<BagItem> BagItems { get; set; }

		/// <summary>
		/// Тела
		/// </summary>
		public List<Body> Bodies { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="slot">Слот</param>
		/// <param name="itemTemplate">Шаблон предмета</param>
		/// <param name="script">Скрипт</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="maxQuantity">Максимальное количество в стаке</param>
		/// <param name="weight">Вес</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Item CreateForTest(
			Guid? id = default,
			ImgFile imgFile = default,
			Slot slot = default,
			ItemTemplate itemTemplate = default,
			Script script = default,
			string name = default,
			string description = default,
			int maxQuantity = default,
			double weight = default)
		=> new Item()
		{
			Id = id ?? Guid.NewGuid(),
			ImgFile = imgFile,
			Slot = slot,
			ItemTemplate = itemTemplate,
			Script = script,
			Name = name ?? "Item",
			Description = description,
			MaxQuantity = maxQuantity,
			Weight = weight
		};
	}
}
