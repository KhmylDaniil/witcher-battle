using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Команда на изменение части шаблона тела
	/// </summary>
	public class ChangeBodyTemplatePartCommand : UpdateBodyTemplateRequestItem, IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<ChangeBodyTemplatePartCommand>(nameof(Name));

			if (!Enum.IsDefined(BodyPartType))
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(BodyPartType));

			if (DamageModifier <= 0)
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(DamageModifier));

			if (HitPenalty < 1)
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(HitPenalty));

			if (MinToHit < 1 || MinToHit > 10)
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(MinToHit),
					"Минимум на попадание должен быть в диапазоне от 1 до 10.");

			if (MaxToHit < 1 || MaxToHit > 10)
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(MaxToHit),
					"Максимум на попадание должен быть в диапазоне от 1 до 10.");

			if (MaxToHit < MinToHit)
				throw new RequestFieldIncorrectDataException<ChangeBodyTemplatePartCommand>(nameof(MaxToHit),
					"Максимум на попадание должен быть больше минимума на попадание");
		}
	}
}
