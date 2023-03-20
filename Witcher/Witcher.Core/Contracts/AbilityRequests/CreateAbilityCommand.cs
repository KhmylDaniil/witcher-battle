using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Запрос создания способности
	/// </summary>
	public class CreateAbilityCommand : IValidatableCommand<Ability>
	{
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

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Навыки для защиты
		/// </summary>
		public List<Skill> DefensiveSkills { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<UpdateAbilityCommandItemAppledCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<CreateAbilityCommand>(nameof(Name));

			if (!Enum.IsDefined(AttackSkill))
				throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(AttackSkill));

			if (AttackDiceQuantity < 1)
				throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(AttackDiceQuantity), "Значение должно быть больше нуля");

			if (AttackSpeed < 1)
				throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(AttackSpeed), "Значение должно быть больше нуля");

			if (!Enum.IsDefined(DamageType))
				throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(DamageType));

			if (DefensiveSkills is not null)
			{
				foreach (var skill in DefensiveSkills)
					if (!Enum.IsDefined(skill))
						throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(DefensiveSkills));
			}

			if (AppliedConditions is not null)
				foreach (var condition in AppliedConditions)
				{
					if (!Enum.IsDefined(condition.Condition))
						throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(AppliedConditions), "Неизвестное накладываемое состояние");

					if (AppliedConditions.Count(x => x.Condition == condition.Condition) != 1)
						throw new RequestNotUniqException<CreateAbilityCommand>(nameof(AppliedConditions));

					if (condition.ApplyChance < 1 || condition.ApplyChance > 100)
						throw new RequestFieldIncorrectDataException<CreateAbilityCommand>(nameof(AppliedConditions), "Шанс наложения состояния должен быть в диапазоне от 1 до 100");
				}
		}
	}
}
