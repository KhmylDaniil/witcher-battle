using System;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class ArmorPart : EntityBase
	{
		private int _currentArmor;
		private Armor _armor;
		private CreaturePart _creaturePart;
		private BodyTemplatePart _bodyTemplatePart;

		/// <summary>
		/// Поле для <see cref="_armor"/>
		/// </summary>
		public const string ArmorField = nameof(_armor);

		/// <summary>
		/// Поле для <see cref="_creaturePart"/>
		/// </summary>
		public const string CreaturePartField = nameof(_creaturePart);

		/// <summary>
		/// Поле для <see cref="_bodyTemplatePart"/>
		/// </summary>
		public const string BodyTemplatePartField = nameof(_bodyTemplatePart);

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public ArmorPart()
		{
		}

		/// <summary>
		/// Конструктор части брони
		/// </summary>
		public ArmorPart(
			Armor armor,
			BodyTemplatePart bodyTemplatePart,
			int currentArmor
			)
		{
			Armor = armor;
			BodyTemplatePart = bodyTemplatePart;
			CurrentArmor = currentArmor;
		}

		public string Name { get; protected set; }

		/// <summary>
		/// Айди брони
		/// </summary>
		public Guid ArmorId { get; protected set; }

		/// <summary>
		/// Айди части существа
		/// </summary>
		public Guid? CreaturePartId { get; protected set; }

		/// <summary>
		/// Айди части шаблона тела
		/// </summary>
		public Guid BodyTemplatePartId { get; protected set; }

		/// <summary>
		/// Текущая броня
		/// </summary>
		public int CurrentArmor
		{
			get => _currentArmor;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<CreaturePart>(nameof(CurrentArmor));
				_currentArmor = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Часть тела существа
		/// </summary>
		public CreaturePart CreaturePart
		{
			get => _creaturePart;
			set
			{
				_creaturePart = value;
				CreaturePartId = value?.Id;
			}
		}

		/// <summary>
		/// Часть шаблона тела
		/// </summary>
		public BodyTemplatePart BodyTemplatePart
		{
			get => _bodyTemplatePart;
			set
			{
				_bodyTemplatePart = value ?? throw new EntityNotIncludedException<ArmorPart>(nameof(BodyTemplatePart));
				BodyTemplatePartId = value.Id;
				Name = value.Name;
			}
		}

		/// <summary>
		/// Броня
		/// </summary>
		public Armor Armor
		{
			get => _armor;
			set
			{
				_armor = value ?? throw new EntityNotIncludedException<ArmorPart>(nameof(Armor));
				ArmorId = value.Id;
			}
		}
		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		[Obsolete("Только для тестов")]
		public static ArmorPart CreateForTest(
			Guid? id = default,
			BodyTemplatePart bodyTemplatePart = default,
			CreaturePart creaturePart = default,
			Armor armor = default,
			int currentArmor = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			BodyTemplatePart = bodyTemplatePart,
			CreaturePart = creaturePart,
			Armor = armor,
			CurrentArmor = currentArmor,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
		};
	}
}
