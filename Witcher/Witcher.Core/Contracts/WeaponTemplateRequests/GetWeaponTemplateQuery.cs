using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class GetWeaponTemplateQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetWeaponTemplateResponse>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		public string AttackSkillName { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public string DamageType { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public string ConditionName { get; set; }

		/// <summary>
		/// Минимальное количество кубов атаки
		/// </summary>
		public int MinAttackDiceQuantity { get; set; }

		/// <summary>
		/// Максимальное количество кубов атаки
		/// </summary>
		public int MaxAttackDiceQuantity { get; set; } = int.MaxValue;

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (MinAttackDiceQuantity < 0)
				throw new RequestFieldIncorrectDataException<GetWeaponTemplateQuery>(nameof(MinAttackDiceQuantity),
					"Значение должно быть больше нуля.");

			if (MaxAttackDiceQuantity < MinAttackDiceQuantity)
				throw new RequestFieldIncorrectDataException<GetWeaponTemplateQuery>(nameof(MaxAttackDiceQuantity),
					"Значение должно быть больше минимального значения фильтра по количеству кубов атаки.");
		}
	}
}
