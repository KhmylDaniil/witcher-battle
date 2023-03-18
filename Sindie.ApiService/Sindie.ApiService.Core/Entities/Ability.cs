using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Способность
	/// </summary>
	public class Ability : EntityBase
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
		public Ability(
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
					throw new ArgumentOutOfRangeException(nameof(AttackDiceQuantity));
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
					throw new ArgumentOutOfRangeException(nameof(AttackSpeed));
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
				_game = value ?? throw new ApplicationException("Необходимо передать игру");
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
		/// <param name="defensiveSkills">Навыки для защиты</param>
		/// <param name="damageType">Типы урона</param>
		/// <param name="appliedConditions">Накладываемые состояния</param>
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
			DamageType damageType,
			List<Skill> defensiveSkills,
			IEnumerable<UpdateAbilityCommandItemAppledCondition> appliedConditions)
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

			ability.UpdateDefensiveSkills(defensiveSkills ?? Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills);
			ability.UpdateAplliedConditions(appliedConditions);

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
		/// <param name="appliedConditions">Накладываемые состояния</param>
		/// <param name="damageType">Типы урона</param>
		/// <param name="defensiveSkills">Защитные навыки</param>
		public void ChangeAbility(
			string name,
			string description,
			Skill attackSkill,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			DamageType damageType,
			List<Skill> defensiveSkills,
			IEnumerable<UpdateAbilityCommandItemAppledCondition> appliedConditions)
		{
			Name = name;
			Description = description;
			AttackSkill = attackSkill;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			DamageType = damageType;

			if (defensiveSkills != null)
				UpdateDefensiveSkills(defensiveSkills);

			UpdateAplliedConditions(appliedConditions);
		}

		/// <summary>
		/// Обновление списка применяемых состояний
		/// </summary>
		/// <param name="data">Данные</param>
		private void UpdateAplliedConditions(IEnumerable<UpdateAbilityCommandItemAppledCondition> data)
		{
			if (data == null)
				return;

			if (AppliedConditions == null)
				throw new ExceptionFieldOutOfRange<Ability>(nameof(AppliedConditions));

			var entitiesToDelete = AppliedConditions.Where(x => !data
				.Any(y => y.Id == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					AppliedConditions.Remove(entity);

			if (!data.Any())
				return;

			foreach (var dataItem in data)
			{
				var appliedCondition = AppliedConditions.FirstOrDefault(x => x.Id == dataItem.Id);
				if (appliedCondition == null)
					AppliedConditions.Add(
						AppliedCondition.CreateAppliedCondition(
							ability: this,
							condition: dataItem.Condition,
							applyChance: dataItem.ApplyChance));
				else
					appliedCondition.ChangeAppliedCondition(
						condition: dataItem.Condition,
						applyChance: dataItem.ApplyChance);
			}
		}

		/// <summary>
		/// Обновление списка защитных навыков
		/// </summary>
		/// <param name="data">Защитные навыки</param>
		private void UpdateDefensiveSkills(List<Skill> data)
			=> DefensiveSkills = data is null
			? throw new ArgumentNullException(nameof(data))
			:data.Select(x => new DefensiveSkill(Id, x)).ToList();

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

	/// <summary>
	/// Защитный навык
	/// </summary>
	public class DefensiveSkill : EntityBase
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		public DefensiveSkill(Guid id, Skill skill)
		{
			AbilityId = id;
			Skill = skill;
		}

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected DefensiveSkill()
		{
		}
	}
}
