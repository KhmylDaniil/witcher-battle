using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Шаблон части тела
	/// </summary>
	public class BodyTemplatePart: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_bodyTemplate"/>
		/// </summary>
		public const string BodyTemplateField = nameof(_bodyTemplate);

		private int _maxToHit;
		private int _minToHit;
		private double _damageModifier;
		private int _hitPenalty;
		private BodyTemplate _bodyTemplate;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected BodyTemplatePart()
		{
		}

		/// <summary>
		/// Конструктор шаблона части тела
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		public BodyTemplatePart(
			BodyTemplate bodyTemplate,
			string name,
			double damageModifier,
			int hitPenalty,
			int minToHit,
			int maxToHit)
		{
			BodyTemplate = bodyTemplate;
			Name = name;
			DamageModifier = damageModifier;
			HitPenalty = hitPenalty;
			MinToHit = minToHit;
			MaxToHit = maxToHit;
		}

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; protected set; } 
		
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
					throw new ArgumentOutOfRangeException(nameof(HitPenalty));
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
		/// Шаблон тела
		/// </summary>
		public BodyTemplate BodyTemplate
		{
			get => _bodyTemplate;
			set
			{
				_bodyTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон тела");
				BodyTemplateId = value.Id;
			}
		}
		#endregion navigation properties


		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="name">Название</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата модификации</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Часть шаблона тела</returns>
		[Obsolete("Только для тестов")]
		public static BodyTemplatePart CreateForTest(
			Guid? id = default,
			BodyTemplate bodyTemplate = default,
			string name = default,
			int hitPenalty = default,
			double damageModifier = default,
			int minToHit = default,
			int maxToHit = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new BodyTemplatePart()
		{
			Id = id ?? Guid.NewGuid(),
			BodyTemplate = bodyTemplate,
			Name = name ?? "BodyTemplate",
			HitPenalty = hitPenalty,
			DamageModifier = damageModifier,
			MinToHit = minToHit,
			MaxToHit = maxToHit,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
