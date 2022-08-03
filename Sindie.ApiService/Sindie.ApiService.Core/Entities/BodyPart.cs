using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть тела - базовая
	/// </summary>
	public class BodyPart: EntityBase
	{
		private int _maxToHit;
		private int _minToHit;
		private double _damageModifier;
		private int _hitPenalty;
		private BodyPartType _bodyPartType;

		/// <summary>
		/// Поле для <see cref="_bodyPartType"/>
		/// </summary>
		public const string BodyPartTypeField = nameof(_bodyPartType);

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
		/// Айди типа части тела
		/// </summary>
		public Guid BodyPartTypeId { get; protected set; }

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
					throw new ExceptionFieldOutOfRange<BodyPart>(nameof(HitPenalty));
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
					throw new ArgumentOutOfRangeException(nameof(DamageModifier));
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
					throw new ArgumentOutOfRangeException(nameof(MinToHit));
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
					throw new ArgumentOutOfRangeException(nameof(MaxToHit));
				_maxToHit = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartType
		{
			get => _bodyPartType;
			set
			{
				_bodyPartType = value ?? throw new ApplicationException("Необходимо передать тип части тела");
				BodyPartTypeId = value.Id;
			}
		}
		#endregion navigation properties
	}
}
