using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть тела существа
	/// </summary>
	public class CreaturePart: BodyPart
	{
		private int _startingArmor;
		private int _currentArmor;
		private Creature _creature;

		/// <summary>
		/// Поле для <see cref="_creature"/>
		/// </summary>
		public const string CreatureField = nameof(_creature);

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public CreaturePart()
		{
		}

		/// <summary>
		/// Конструктор части существа
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="armor">Броня</param> 
		public CreaturePart(
			Creature creature,
			BodyPartType bodyPartType,
			string name,
			double damageModifier,
			int hitPenalty,
			int minToHit,
			int maxToHit,
			int armor
			)
			: base(
			bodyPartType,
			name,
			damageModifier,
			hitPenalty,
			minToHit,
			maxToHit)
		{
			Creature = creature;
			StartingArmor = armor;
			CurrentArmor = armor;
		}

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

		/// <summary>
		/// Начальная броня
		/// </summary>
		public int StartingArmor
		{
			get => _startingArmor;
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(StartingArmor));
				_startingArmor = value;
			}
		}
		/// <summary>
		/// Текущая броня
		/// </summary>
		public int CurrentArmor
		{
			get => _currentArmor;
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(CurrentArmor));
				_currentArmor = value;
			}
		}

		#region navigation properties
		/// <summary>
		/// Существо
		/// </summary>
		public Creature Creature
		{
			get => _creature;
			set
			{
				_creature = value ?? throw new ApplicationException("Необходимо передать существо");
				CreatureId = value.Id;
			}
		}
		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creature">Шаблон существа</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="name">Название</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти на попадание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="startingArmor">Стартовая броня</param>
		/// <param name="currentArmor">Текущая броня</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static CreaturePart CreateForTest(
			Guid? id = default,
			Creature creature = default,
			BodyPartType bodyPartType = default,
			string name = default,
			double damageModifier = default,
			int hitPenalty = default,
			int minToHit = default,
			int maxToHit = default,
			int startingArmor = default,
			int currentArmor = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			BodyPartType = bodyPartType,
			Name = name ?? Enum.GetName(bodyPartType),
			DamageModifier = damageModifier,
			HitPenalty = hitPenalty,
			MinToHit = minToHit,
			MaxToHit = maxToHit,
			StartingArmor = startingArmor,
			CurrentArmor = currentArmor,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
		};
	}
}
