using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Abstractions;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Contracts.ItemTemplateBase;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class CreateWeaponTemplateCommand : CreateOrUpdateItemTemplateCommandBase, IValidatableCommand<Guid>
	{
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
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Durability { get; set; }

		/// <summary>
		/// Дальность
		/// </summary>
		public int? Range { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<UpdateAttackFormulaCommandItemAppledCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public override void Validate()
		{
			base.Validate();
			
			if (!Enum.IsDefined(AttackSkill))
				throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(AttackSkill));

			if (AttackDiceQuantity < 1)
				throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(AttackDiceQuantity), "Значение должно быть больше нуля");

			if (Durability < 1)
				throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(Durability), "Значение должно быть больше нуля");

			if (Range is not null && Range < 2)
				throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(Range), "Значение должно быть больше минимального");

			if (!Enum.IsDefined(DamageType))
				throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(DamageType));

			if (AppliedConditions is not null)
				foreach (var condition in AppliedConditions)
				{
					if (!Enum.IsDefined(condition.Condition))
						throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(AppliedConditions), "Неизвестное накладываемое состояние");

					if (AppliedConditions.Count(x => x.Condition == condition.Condition) != 1)
						throw new RequestNotUniqException<CreateWeaponTemplateCommand>(nameof(AppliedConditions));

					if (condition.ApplyChance < 1 || condition.ApplyChance > 100)
						throw new RequestFieldIncorrectDataException<CreateWeaponTemplateCommand>(nameof(AppliedConditions), "Шанс наложения состояния должен быть в диапазоне от 1 до 100");
				}
		}
	}
}
