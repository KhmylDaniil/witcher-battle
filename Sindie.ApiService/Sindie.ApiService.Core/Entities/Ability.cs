using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.AbilityRequests;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Поле для <see cref="_attackParameter"/>
		/// </summary>
		public const string ParameterField = nameof(_attackParameter);

		private Game _game;
		private Parameter _attackParameter;
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
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="defensiveParameters">Параметры для защиты</param>
		/// <param name="damageTypes">Типы урона</param>
		public Ability(
			Game game,
			string name,
			string description,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			Parameter attackParameter,
			List<Parameter> defensiveParameters,
			List<DamageType> damageTypes)
		{
			Game = game;
			Name = name;
			Description = description;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			AttackParameter = attackParameter;
			AppliedConditions = new List<AppliedCondition>();
			Creatures = new List<Creature>();
			DefensiveParameters = defensiveParameters;
			DamageTypes = damageTypes;
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
		/// Айди параметра атаки
		/// </summary>
		public Guid AttackParameterId { get; protected set; }

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
		/// Параметр атаки
		/// </summary>
		public Parameter AttackParameter
		{
			get => _attackParameter;
			protected set
			{
				_attackParameter = value ?? throw new ApplicationException("Необходимо передать параметр");
				AttackParameterId = value.Id;
			}
		}

		/// <summary>
		/// Параметры для защиты
		/// </summary>
		public List<Parameter> DefensiveParameters { get; protected set; }

		/// <summary>
		/// Типы урона
		/// </summary>
		public List<DamageType> DamageTypes { get; protected set; }

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
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="defensiveParameters">Параметры для защиты</param>
		/// <param name="damageTypes">Типы урона</param>
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
			Parameter attackParameter,
			List<Parameter> defensiveParameters,
			List<DamageType> damageTypes,
			List<AppliedConditionData> appliedConditions)
		{
			var ability = new Ability(
				game: game,
				name: name,
				description: description,
				attackParameter: attackParameter,
				attackDiceQuantity: attackDiceQuantity,
				damageModifier: damageModifier,
				attackSpeed: attackSpeed,
				accuracy: accuracy,
				defensiveParameters: defensiveParameters,
				damageTypes: damageTypes);

			ability.UpdateAplliedConditions(appliedConditions);

			return ability;
		}

		/// <summary>
		/// Изменение способности
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="appliedConditions">Накладываемые состояния</param>
		public void ChangeAbility(
			string name,
			string description,
			Parameter attackParameter,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			List<Parameter> defensiveParameters,
			List<DamageType> damageTypes,
			List<AppliedConditionData> appliedConditions)
		{
			Name = name;
			Description = description;
			AttackParameter = attackParameter;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			DefensiveParameters = defensiveParameters;
			DamageTypes = damageTypes;
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
		/// Расчет урона от атаки
		/// </summary>
		/// <returns>Нанесенный урон</returns>
		internal int RollDamage(int specialBonus = default)
		{
			Random random = new Random();
			var result = DamageModifier + specialBonus;
			for (int i = 0; i < AttackDiceQuantity; i++)
				result += random.Next(1, 6);
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Расчет применения состояний
		/// </summary>
		/// <returns>Наложенные состояния</returns>
		internal List<Condition> RollConditions()
		{
			var result = new List<Condition>();
			Random random = new Random();

			if (!AppliedConditions.Any())
				return result;

			foreach (var condition in AppliedConditions)
				if (random.Next(1, 100) <= condition.ApplyChance)
					result.Add(condition.Condition);

			return result;
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
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
			Parameter attackParameter = default,
			List<Parameter> defensiveParameters = default,
			List<DamageType> damageTypes = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Ability()
		{
			Id = id ?? Guid.NewGuid(),
			Game = game,
			Name = name ?? "name",
			Description = description,
			AttackDiceQuantity = attackDiceQuantity,
			DamageModifier = damageModifier,
			AttackSpeed = attackSpeed,
			Accuracy = accuracy,
			AttackParameter = attackParameter,
			DefensiveParameters = defensiveParameters ?? new List<Parameter>(),
			DamageTypes = damageTypes ?? new List<DamageType>(),
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			Creatures = new List<Creature>(),
			AppliedConditions = new List<AppliedCondition>()
		};
	}
}
