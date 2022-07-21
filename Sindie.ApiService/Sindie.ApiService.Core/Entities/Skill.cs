using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Навык
	/// </summary>
	public class Skill : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;
		private string _statName;

		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected Skill()
		{
		}

		/// <summary>
		/// Конструктор навыка
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="minValueSkill">Минимальное значение навыка</param>
		/// <param name="maxValueSkill">Максимальное значение навыка</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		/// <param name="statName">Название корреспондирующей характеристики</param>
		public Skill(
			Game game,
			int minValueSkill,
			int maxValueSkill,
			string name,
			string description,
			string statName)
		{
			Game = game;
			Name = name;
			Description = description;
			SkillBounds = new SkillBound(
				minValueSkill,
				maxValueSkill);
			CreatureSkills = new List<CreatureSkill>();
			CreatureTemplateSkills = new List<CreatureTemplateSkill>();
			AbilitiesForAttack = new List<Ability>();
			AbilitiesForDefense = new List<Ability>();
			StatName = statName;
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Название навыка
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание навыка
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Название корреспондирующей характеристики
		/// </summary>
		public string StatName
		{
			get => _statName;
			set => _statName = BaseData.Stats.StatsList.Contains(value) ? value : string.Empty;
		}

		/// <summary>
		/// Границы навыка
		/// </summary>
		public SkillBound SkillBounds { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new ApplicationException("Необходимо передать игру");
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Навыки шаблона существа
		/// </summary>
		public List<CreatureTemplateSkill> CreatureTemplateSkills { get; set; }

		/// <summary>
		/// Навыки существа
		/// </summary>
		public List<CreatureSkill> CreatureSkills { get; set; }

		/// <summary>
		/// Способности, в которых используется для атаки
		/// </summary>
		public List<Ability> AbilitiesForAttack { get; set; }

		/// <summary>
		/// Способности, в которых используется для защиты
		/// </summary>
		public List<Ability> AbilitiesForDefense { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Изменение навыка
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="minValueSkill">МИнимальное значение</param>
		/// <param name="maxValueSkill">Максимальное значение</param>
		/// <param name="statName">Название корреспондирующей характеристики</param>
		public void ChangeSkill(
			Game game,
			string name,
			string description,
			int minValueSkill,
			int maxValueSkill,
			string statName)
		{
			Game = game;
			Name = name;
			Description = description;
			StatName = statName;
			SkillBounds = new SkillBound(
				minValueSkill,
				maxValueSkill);
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="game">Игра</param>
		/// <param name="description">Описание</param>
		/// <param name="minValueSkill">Минимальное значение</param>
		/// <param name="maxValueSkill">Максимальное значение</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <param name="statName">Название корреспондирующей характеристики</param>
		/// <returns>Навык</returns>
		[Obsolete("Только для тестов")]
		public static Skill CreateForTest(
			Guid? id = default,
			string name = default,
			Game game = default,
			string description = default,
			string statName = default,
			int minValueSkill = default,
			int maxValueSkill = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Skill()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Skill",
			Game = game,
			Description = description,
			StatName = statName ?? "Ref",
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			SkillBounds = (minValueSkill == 0 && maxValueSkill == 0)
			? new SkillBound(1, 10)
			: new SkillBound(
				minValueSkill,
				maxValueSkill),
			AbilitiesForAttack = new List<Ability>(),
			AbilitiesForDefense = new List<Ability>(),
			CreatureSkills = new List<CreatureSkill>(),
			CreatureTemplateSkills = new List<CreatureTemplateSkill>()
		};

		/// <summary>
		/// Ограничения параметра
		/// </summary>
		public class SkillBound
		{
			/// <summary>
			/// Пустой конструктор для EF
			/// </summary>
			protected SkillBound()
			{
			}

			/// <summary>
			/// Конструктор класса Ограничения навыка
			/// </summary>
			/// <param name="minValueSkill">Минимальное значение навыка</param>
			/// <param name="maxValueSkill">Максимальное значение навыка</param>
			public SkillBound(
				int minValueSkill,
				int maxValueSkill)
			{
				MinValueSkill = minValueSkill;
				MaxValueSkill = maxValueSkill;
			}

			/// <summary>
			/// Минимальное значение навыка
			/// </summary>
			public int MinValueSkill { get; set; }

			/// <summary>
			/// Максимальное значение навыка
			/// </summary>
			public int MaxValueSkill { get; set; }
		}
	}
}
