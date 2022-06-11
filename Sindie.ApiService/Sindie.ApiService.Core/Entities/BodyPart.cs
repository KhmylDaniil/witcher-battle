
using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть тела
	/// </summary>
	public class BodyPart
	{
		private int _maxToHit;
		private int _minToHit;
		private double _damageModifier;
		private int _hitPenalty;

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
					throw new ArgumentOutOfRangeException(nameof(DamageModifier));
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

		/// <summary>
		/// Начальная броня
		/// </summary>
		public int StartingArmor { get; set; }

		/// <summary>
		/// Текущая броня
		/// </summary>
		public int CurrentArmor { get; set; }
	}
}
