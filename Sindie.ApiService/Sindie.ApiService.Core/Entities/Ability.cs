using System;
using System.Collections.Generic;

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
		/// <param name="appliedConditions">Список применяемых состояний</param>
		/// <param name="attackParameter">Параметр атаки</param>
		public Ability(
			CreatureTemplate creatureTemplate,
			string name,
			string description,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			Parameter attackParameter,
			List<AppliedCondition> appliedConditions)

		{
			CreatureTemplate = creatureTemplate;
			Name = name;
			Description = description;
			AttackDiceQuantity = attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed;
			Accuracy = accuracy;
			AppliedConditions = appliedConditions;
			AttackParameter = attackParameter;
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
	}
}
