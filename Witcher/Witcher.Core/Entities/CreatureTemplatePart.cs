using System;
using Witcher.Core.Exceptions.EntityExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Часть шаблона существа
	/// </summary>
	public class CreatureTemplatePart: BodyPart
	{
		private int _armor;
		private CreatureTemplate _creatureTemplate;

		/// <summary>
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public CreatureTemplatePart()
		{
		}

		/// <summary>
		/// Конструктор части шаблона существа
		/// </summary>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="name">Название</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="armor">Броня</param> 
		public CreatureTemplatePart(
			CreatureTemplate creatureTemplate,
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
			CreatureTemplate = creatureTemplate;
			Armor = armor;
		}

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor
		{
			get => _armor;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<CreatureTemplatePart>(nameof(Armor));
				_armor = value;
			}
		}

		#region navigation properties
		/// <summary>
		/// Шаблон существа
		/// </summary>
		public CreatureTemplate CreatureTemplate
		{
			get => _creatureTemplate;
			set
			{
				_creatureTemplate = value ?? throw new EntityNotIncludedException<CreatureTemplatePart>(nameof(CreatureTemplate));
				CreatureTemplateId = value.Id;
			}
		}
		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="name">Название</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти на попадание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="armor">Броня</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static CreatureTemplatePart CreateForTest(
			Guid? id = default,
			CreatureTemplate creatureTemplate = default,
			BodyPartType bodyPartType = default,
			string name = default,
			double damageModifier = default,
			int hitPenalty = default,
			int minToHit = default,
			int maxToHit = default,
			int armor = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			CreatureTemplate = creatureTemplate,
			BodyPartType = bodyPartType,
			Name = name ?? Enum.GetName(bodyPartType),
			DamageModifier = damageModifier == 0 ? 1 : damageModifier,
			HitPenalty = hitPenalty == 0 ? 1 : hitPenalty,
			MinToHit = minToHit == 0 ? 1 : minToHit,
			MaxToHit = maxToHit == 0 ? 10 : maxToHit,
			Armor = armor,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
		};
	}
}
