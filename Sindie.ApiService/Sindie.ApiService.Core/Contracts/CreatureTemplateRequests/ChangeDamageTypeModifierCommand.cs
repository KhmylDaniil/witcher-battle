using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда изменения модификатора урона по типу 
	/// </summary>
	public class ChangeDamageTypeModifierCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

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
				throw new RequestFieldIncorrectDataException<ChangeDamageTypeModifierCommand>(nameof(DamageType));

			if (!Enum.IsDefined(DamageTypeModifier))
				throw new RequestFieldIncorrectDataException<ChangeDamageTypeModifierCommand>(nameof(DamageTypeModifier));
		}
	}
}
