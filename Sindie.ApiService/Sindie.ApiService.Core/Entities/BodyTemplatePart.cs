using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть шаблона тела
	/// </summary>
	public class BodyTemplatePart: BodyPart
	{
		private BodyTemplate _bodyTemplate;

		/// <summary>
		/// Поле для <see cref="_bodyTemplate"/>
		/// </summary>
		public const string BodyTemplateField = nameof(_bodyTemplate);

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public BodyTemplatePart()
		{
		}

		/// <summary>
		/// Конструктор для класса часть шаблона тела
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="name">Название</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти за прицеливание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		public BodyTemplatePart(
			BodyTemplate bodyTemplate,
			BodyPartType bodyPartType,
			string name,
			double damageModifier,
			int hitPenalty,
			int minToHit,
			int maxToHit
			)
			: base(
			bodyPartType,
			name,
			damageModifier,
			hitPenalty,
			minToHit,
			maxToHit)
		{
			BodyTemplate = bodyTemplate;
		}

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; protected set; }

		#region navigation properties
		/// <summary>
		/// Шаблон тела
		/// </summary>
		public BodyTemplate BodyTemplate
		{
			get => _bodyTemplate;
			protected set
			{
				_bodyTemplate = value ?? throw new ApplicationException("Необходимо передать тип части тела");
				BodyPartTypeId = value.Id;
			}
		}
		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="name">Название</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="hitPenalty">Пенальти на попадание</param>
		/// <param name="minToHit">Минимум на попадание</param>
		/// <param name="maxToHit">Максимум на попадание</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static BodyTemplatePart CreateForTest(
			Guid? id = default,
			BodyTemplate bodyTemplate = default,
			BodyPartType bodyPartType = default,
			string name = default,
			int damageModifier = default,
			int hitPenalty = default,
			int minToHit = default,
			int maxToHit = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new BodyTemplatePart()
		{
			Id = id ?? Guid.NewGuid(),
			BodyTemplate = bodyTemplate,
			BodyPartType = bodyPartType,
			Name = name ?? "name",
			DamageModifier = damageModifier,
			HitPenalty = hitPenalty,
			MinToHit = minToHit,
			MaxToHit = maxToHit,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
		};
	}
}
