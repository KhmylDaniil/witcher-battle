using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
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

		private Creature _creature;
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
			MaxValue = skillValue;
		}

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; protected set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Максималальное значение навыка
		/// </summary>
		public int MaxValue { get; private set; }

		/// <summary>
		/// Значение навыка у существа
		/// </summary>
		public int SkillValue
		{
			get => _skillValue < 0 ? 0 : _skillValue;
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
			int maxValue = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new CreatureSkill()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			Skill = skill,
			SkillValue = value == 0 ? 1 : value,
			MaxValue = maxValue == 0 ? 1 : maxValue,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};

		internal int GetValue() => _skillValue;
	}
}
