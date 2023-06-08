using System.Collections;
using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class GetArmorTemplateQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetArmorTemplateResponse>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Фильтр по типу закрываемой броней части тела
		/// </summary>
		public string BodyPartType { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public string DamageTypeModifier { get; set; }

		/// <summary>
		/// Минимальное количество брони
		/// </summary>
		public int MinArmor { get; set; }

		/// <summary>
		/// Максимальное количество брони
		/// </summary>
		public int MaxArmor { get; set; } = int.MaxValue;

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (MinArmor < 0)
				throw new RequestFieldIncorrectDataException<GetArmorTemplateQuery>(nameof(MinArmor),
					"Значение должно быть больше нуля.");

			if (MaxArmor < MinArmor)
				throw new RequestFieldIncorrectDataException<GetArmorTemplateQuery>(nameof(MaxArmor),
					"Значение должно быть больше минимального значения фильтра по количеству брони.");
		}
	}
}
