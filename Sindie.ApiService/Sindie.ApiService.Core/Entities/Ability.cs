using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
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
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		/// <summary>
		/// Поле для <see cref="_attackParameter"/>
		/// </summary>
		public const string ParameterField = nameof(_attackParameter);

		private CreatureTemplate _creatureTemplate;
		private Parameter _attackParameter;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Ability()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackDiceQuantity">Количество кубиков атаки</param>
		/// <param name="damageModifier">Модификатор урона</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="attackParameter">Параметр атаки</param>
		public Ability(
			CreatureTemplate creatureTemplate,
			string name,
			string description,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			Parameter attackParameter)
		{
			CreatureTemplate = creatureTemplate;
			Name = name;
			Description = description;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			AttackParameter = attackParameter;
			AppliedConditions = new List<AppliedCondition>();
			Creatures = new List<Creature>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

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
		public Guid AttackParameterId { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity { get; set; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Скорость атаки
		/// </summary>
		public int AttackSpeed { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		#region navigation properties

		/// <summary>
		/// Шаблон персонажа
		/// </summary>
		public CreatureTemplate CreatureTemplate
		{
			get => _creatureTemplate;
			protected set
			{
				_creatureTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон существа");
				CreatureTemplateId = value.Id;
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
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackParameter">Параметр атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор атаки</param>
		/// <param name="attackSpeed">Скорость атааки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="appliedConditions">Накладываемые состояния</param>
		/// <returns>Способность</returns>
		public static Ability CreateAbility(
			CreatureTemplate creatureTemplate,
			string name,
			string description,
			Parameter attackParameter,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			List<(Guid? Id, Condition Condition, double ApplyChance)> appliedConditions)
		{
			var ability = new Ability(
				creatureTemplate: creatureTemplate,
				name: name,
				description: description,
				attackParameter: attackParameter,
				attackDiceQuantity: attackDiceQuantity,
				damageModifier: damageModifier,
				attackSpeed: attackSpeed,
				accuracy: accuracy);
			
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
			List<(Guid? Id, Condition Condition, double ApplyChance)> appliedConditions)
		{
			Name = name;
			Description = description;
			AttackParameter = attackParameter;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			UpdateAplliedConditions(appliedConditions);
		}

		/// <summary>
		/// Обновление списка применяемых состояний
		/// </summary>
		/// <param name="data">Данные</param>
		public void UpdateAplliedConditions(List<(Guid? Id, Condition Condition, double ApplyChance)> data)
		{
			if (data == null)
				throw new ExceptionRequestFieldNull<AbilityData>(nameof(AbilityData.AppliedConditions));

			if (AppliedConditions == null)
				throw new ExceptionEntityNotIncluded<AppliedCondition>(nameof(Ability));

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
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creatureTemplate">Шабблон существа</param>
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
			CreatureTemplate creatureTemplate = default,
			string name = default,
			string description = default,
			int attackDiceQuantity = default,
			int damageModifier = default,
			int attackSpeed = default,
			int accuracy = default,
			Parameter attackParameter = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Ability()
		{
			Id = id ?? Guid.NewGuid(),
			CreatureTemplate = creatureTemplate,
			Name = name,
			Description = description,
			AttackDiceQuantity = attackDiceQuantity,
			DamageModifier = damageModifier,
			AttackSpeed = attackSpeed,
			Accuracy = accuracy,
			AttackParameter = attackParameter,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			Creatures = new List<Creature>(),
			AppliedConditions = new List<AppliedCondition>()
		};
	}
}
