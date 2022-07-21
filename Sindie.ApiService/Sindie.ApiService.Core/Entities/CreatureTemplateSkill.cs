using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Навык существа
	/// </summary>
	public class CreatureTemplateSkill : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		/// <summary>
		/// Поле для <see cref="_skill"/>
		/// </summary>
		public const string SkillField = nameof(_skill);

		private CreatureTemplate _creatureTemplate;
		private Skill _skill;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureTemplateSkill()
		{
		}

		/// <summary>
		/// Навык швблона существа
		/// </summary>
		/// <param name="skillValue">Значение навыка шаблона существа</param>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="skill">Навык</param>
		public CreatureTemplateSkill(
			int skillValue,
			CreatureTemplate creatureTemplate,
			Skill skill)
		{
			CreatureTemplate = creatureTemplate;
			Skill = skill;
			SkillValue = skillValue;
		}

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

		/// <summary>
		/// Айди навыка
		/// </summary>
		public Guid SkillId { get; protected set; }

		/// <summary>
		/// Название корреспондирующей характеристики
		/// </summary>
		public string StatName { get; protected set; }

		/// <summary>
		/// Значение навыка у шаблона существа
		/// </summary>
		public int SkillValue { get; set; }

		#region navigation properties

		/// <summary>
		/// Шаблон существа
		/// </summary>
		public CreatureTemplate CreatureTemplate
		{
			get => _creatureTemplate;
			set
			{
				_creatureTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон существа");
				CreatureTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill
		{
			get => _skill;
			set
			{
				_skill = value ?? throw new ApplicationException("Необходимо передать навык");
				SkillId = value.Id;
				StatName = value.StatName;
			}
		}

		#endregion navigation properties

		/// <summary>
		/// изменить значение навыка шаблона существа
		/// </summary>
		/// <param name="value">зЗначение навыка</param>
		public void ChangeValue(int value)
		{
			SkillValue = value;
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="skill">Параметр</param>
		/// <param name="value">Значение навыка</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Навык шаблона существа</returns>
		[Obsolete("Только для тестов")]
		public static CreatureTemplateSkill CreateForTest(
			Guid? id = default,
			CreatureTemplate creatureTemplate = default,
			Skill skill = default,
			int value = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new CreatureTemplateSkill()
		{
			Id = id ?? Guid.NewGuid(),
			CreatureTemplate = creatureTemplate,
			Skill = skill,
			SkillValue = value == 0 ? 1 : value,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
