using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Навык существа
	/// </summary>
	public class CreatureSkill: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_creature"/>
		/// </summary>
		public const string CreatureField = nameof(_creature);

		/// <summary>
		/// Поле для <see cref="_skill"/>
		/// </summary>
		public const string SkillField = nameof(_skill);

		private Creature _creature;
		private Skill _skill;
		private int _skillValue;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureSkill()
		{
		}

		/// <summary>
		/// Навык существа
		/// </summary>
		/// <param name="skillValue">Значение навыка существа</param>
		/// <param name="creature">Cущество</param>
		/// <param name="skill">Навык</param>
		public CreatureSkill(
			int skillValue,
			Creature creature,
			Skill skill)
		{
			Creature = creature;
			Skill = skill;
			SkillValue = skillValue;
		}

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

		/// <summary>
		/// Айди навыка
		/// </summary>
		public Guid SkillId { get; protected set; }

		/// <summary>
		/// Название корреспондирующей характеристики
		/// </summary>
		public string StatName { get; protected set; }

		/// <summary>
		/// значение навыка у существа
		/// </summary>
		public int SkillValue
		{
			get => _skillValue < 1 ? 1 : _skillValue;
			set => _skillValue = value;
		}

		#region navigation properties

		/// <summary>
		/// Шаблон существа
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
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creature">Существо</param>
		/// <param name="skill">Навык</param>
		/// <param name="value">Значение навыка</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Навык существа</returns>
		[Obsolete("Только для тестов")]
		public static CreatureSkill CreateForTest(
			Guid? id = default,
			Creature creature = default,
			Skill skill = default,
			int value = default,
			string statName = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new CreatureSkill()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			Skill = skill,
			SkillValue = value == 0 ? 1 : value,
			StatName = statName ?? "Ref",
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};

		internal int GetValue() => _skillValue;
	}
}
