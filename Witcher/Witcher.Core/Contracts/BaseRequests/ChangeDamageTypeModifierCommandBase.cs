using System;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.BaseRequests
{
	public class ChangeDamageTypeModifierCommandBase
	{
		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public DamageTypeModifier DamageTypeModifier { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (!Enum.IsDefined(DamageType))
				throw new RequestFieldIncorrectDataException<ChangeDamageTypeModifierForCreatureTemplateCommand>(nameof(DamageType));

			if (!Enum.IsDefined(DamageTypeModifier))
				throw new RequestFieldIncorrectDataException<ChangeDamageTypeModifierForCreatureTemplateCommand>(nameof(DamageTypeModifier));
		}
	}
}
