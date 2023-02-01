using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.AbilityRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities
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

		/// <summary>
		/// Поле для <see cref="_damageType"/>
		/// </summary>
		public const string DamageTypeField = nameof(_damageType);

		private Game _game;
		private int _attackDiceQuantity;
		private int _attackSpeed;
		private DamageType _damageType;

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
		/// <param name="defensiveSkills">Навыки для защиты</param>
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
			DamageType damageType,
			List<Skill> defensiveSkills)
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
			DefensiveSkills = defensiveSkills;
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
		/// Айди типа урона
		/// </summary>
		public Guid DamageTypeId { get; protected set; }

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

		/// <summary>
		/// Навыки для защиты
		/// </summary>
		public List<Skill> DefensiveSkills { get; set; }

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
		/// Тип урона
		/// </summary>
		public DamageType DamageType
		{
			get => _damageType;
			protected set
			{
				_damageType = value ?? throw new ApplicationException("Необходимо передать тип урона");
				DamageTypeId = value.Id;
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
			List<AppliedConditionData> appliedConditions)
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
				defensiveSkills: defensiveSkills,
				damageType: damageType);

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
			List<AppliedConditionData> appliedConditions)
		{
			Name = name;
			Description = description;
			AttackSkill = attackSkill;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			DefensiveSkills = defensiveSkills;
			DamageType = damageType;
			UpdateAplliedConditions(appliedConditions);
		}

		/// <summary>
		/// Обновление списка применяемых состояний
		/// </summary>
		/// <param name="data">Данные</param>
		private void UpdateAplliedConditions(List<AppliedConditionData> data)
		{
			if (data == null)
				throw new ApplicationException("Не переданы данные для обновления накладываемых состояний");

			if (AppliedConditions == null)
				throw new ExceptionFieldOutOfRange<Ability>(nameof(AppliedConditions));

			var entitiesToDelete = AppliedConditions.Where(x => !data
				.Any(y => y.AppliedConditionId == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					AppliedConditions.Remove(entity);

			if (!data.Any())
				return;

			foreach (var dataItem in data)
			{
				var appliedCondition = AppliedConditions.FirstOrDefault(x => x.Id == dataItem.AppliedConditionId);
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
			Skill attackSkill = default,
			List<Skill> defensiveSkills = default,
			DamageType damageType = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new ()
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
			DefensiveSkills = defensiveSkills ?? new List<Skill>(),
			DamageType = damageType ?? DamageType.CreateForTest(),
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			Creatures = new List<Creature>(),
			AppliedConditions = new List<AppliedCondition>()
		};
	}
}
