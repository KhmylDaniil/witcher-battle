using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Часть тела - базовая
	/// </summary>
	public class BodyPart: EntityBase, IBodyTemplatePartData
	{
		private int _maxToHit;
		private int _minToHit;
		private double _damageModifier;
		private int _hitPenalty;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected BodyPart()
		{
		}

		/// <summary>
		/// Конструктор части тела
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>

		public BodyPart(
			BodyPartType bodyPartType,
			string name,
			double damageModifier,
			int hitPenalty,
			int minToHit,
			int maxToHit)
		{
			BodyPartType = bodyPartType;
			Name = name;
			DamageModifier = damageModifier;
			HitPenalty = hitPenalty;
			MinToHit = minToHit;
			MaxToHit = maxToHit;
		}

		/// <summary>
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartType { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		public int HitPenalty
		{
			get => _hitPenalty;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<BodyPart>(nameof(HitPenalty));
				_hitPenalty = value;
			}
		}

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifier
		{
			get => _damageModifier;
			set
			{
				if (value <= 0)
					throw new FieldOutOfRangeException<BodyPart>(nameof(DamageModifier));
				_damageModifier = value;
			}
		}

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		public int MinToHit
		{
			get => _minToHit;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<BodyPart>(nameof(MinToHit));
				_minToHit = value;
			}
		}

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		public int MaxToHit
		{
			get => _maxToHit;
			set
			{
				if (value < MinToHit || value > 10)
					throw new FieldOutOfRangeException<BodyPart>(nameof(MaxToHit));
				_maxToHit = value;
			}
		}
	}
}
