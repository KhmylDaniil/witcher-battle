using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.SystemExceptions;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Способность
	/// </summary>
	public sealed class Ability : EntityBase, IAttackFormula
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;
		private int _attackDiceQuantity;
		private int _attackSpeed;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Ability()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackDiceQuantity">Количество кубиков атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="attackSkill">Навык атаки</param>
		/// <param name="damageType">Типы урона</param>
		private Ability(
			Game game,
			string name,
			string description,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			Skill attackSkill,
			DamageType damageType)
		{
			Game = game;
			Name = name;
			Description = description;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			AttackSkill = attackSkill;
			AppliedConditions = new List<AppliedCondition>();
			Creatures = new List<Creature>();
			DefensiveSkills = new List<DefensiveSkill>();
			DamageType = damageType;
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Наазвание способности
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание способности
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity
		{
			get => _attackDiceQuantity;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Ability>(nameof(AttackDiceQuantity));
				_attackDiceQuantity = value;
			}
		}

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Скорость атаки
		/// </summary>
		public int AttackSpeed
		{
			get => _attackSpeed;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Ability>(nameof(AttackSpeed));
				_attackSpeed = value;
			}
		}

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<Ability>(nameof(Game));
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Шаблоны существ
		/// </summary>
		public List<CreatureTemplate> CreatureTemplates { get; protected set; }
		
		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Навыки для защиты
		/// </summary>
		public List<DefensiveSkill> DefensiveSkills { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создание способности
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackDiceQuantity">Количество кубиков атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="attackSkill">Навык атаки</param>
		/// <param name="damageType">Типы урона</param>
		/// <returns>Способность</returns>
		public static Ability CreateAbility(
			Game game,
			string name,
			string description,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			Skill attackSkill,
			DamageType damageType)
		{
			var ability = new Ability(
				game: game,
				name: name,
				description: description,
				attackSkill: attackSkill,
				attackDiceQuantity: attackDiceQuantity,
				damageModifier: damageModifier,
				attackSpeed: attackSpeed,
				accuracy: accuracy,
				damageType: damageType);

			ability.UpdateDefensiveSkills(Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills);
			ability.AppliedConditions = new();

			return ability;
		}

		/// <summary>
		/// Изменение способности
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackSkill">Навык атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="damageType">Типы урона</param>
		public void ChangeAbility(
			string name,
			string description,
			Skill attackSkill,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			DamageType damageType)
		{
			Name = name;
			Description = description;
			AttackSkill = attackSkill;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			DamageType = damageType;
		}

		/// <summary>
		/// Обновление списка защитных навыков
		/// </summary>
		/// <param name="data">Защитные навыки</param>
		private void UpdateDefensiveSkills(List<Skill> data)
			=> DefensiveSkills = data is null
			? throw new ApplicationSystemNullException<Ability>(nameof(data))
			:data.Select(x => new DefensiveSkill(x)).ToList();

		/// <summary>
		/// Создание тестовой сущности
		/// </summary>
		/// <param name="id">айди</param>
		/// <param name="game">Игра</param>
		/// <param name="name">название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность</param>
		/// <param name="attackSkill">Навык атаки</param>
		/// <param name="defensiveSkills">Защитные навыки</param>
		/// <param name="damageType">Тип урона</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата модификации</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Способность</returns>
		[Obsolete("Только для тестов")]
		public static Ability CreateForTest(
			Guid? id = default,
			Game game = default,
			string name = default,
			string description = default,
			int attackDiceQuantity = default,
			int damageModifier = default,
			int attackSpeed = default,
			int accuracy = default,
			Skill attackSkill = Skill.Melee,
			List<Skill> defensiveSkills = default,
			DamageType damageType = DamageType.Slashing,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		{
			Ability ability = new()
			{
				Id = id ?? Guid.NewGuid(),
				Game = game,
				Name = name ?? "name",
				Description = description,
				AttackDiceQuantity = attackDiceQuantity,
				DamageModifier = damageModifier,
				AttackSpeed = attackSpeed,
				Accuracy = accuracy,
				AttackSkill = attackSkill,
				DefensiveSkills = new List<DefensiveSkill>(),
				DamageType = damageType,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId,
				Creatures = new List<Creature>(),
				AppliedConditions = new List<AppliedCondition>()
			};
			ability.UpdateDefensiveSkills(defensiveSkills ?? Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills);
			return ability;
		}
	}
}
