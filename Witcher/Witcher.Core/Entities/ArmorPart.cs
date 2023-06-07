using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class ArmorPart : EntityBase
	{
		private int _currentArmor;
		private Armor _armor;
		private CreaturePart _creaturePart;

		/// <summary>
		/// Поле для <see cref="_armor"/>
		/// </summary>
		public const string ArmorField = nameof(_armor);

		/// <summary>
		/// Поле для <see cref="_creaturePart"/>
		/// </summary>
		public const string CreaturePartField = nameof(_creaturePart);

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
			CreaturePart creaturePart,
			int currentArmor
			)
		{
			Armor = armor;
			CreaturePart = creaturePart;
			CurrentArmor = currentArmor;
		}

		/// <summary>
		/// Айди брони
		/// </summary>
		public Guid ArmorId { get; protected set; }

		/// <summary>
		/// Айди части существа
		/// </summary>
		public Guid CreaturePartId { get; protected set; }

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
				_creaturePart = value ?? throw new EntityNotIncludedException<ArmorPart>(nameof(CreaturePart));
				CreaturePartId = value.Id;
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
			CreaturePart creaturePart = default,
			Armor armor = default,
			int currentArmor = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			CreaturePart = creaturePart,
			Armor = armor,
			CurrentArmor = currentArmor,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
		};
	}
}
