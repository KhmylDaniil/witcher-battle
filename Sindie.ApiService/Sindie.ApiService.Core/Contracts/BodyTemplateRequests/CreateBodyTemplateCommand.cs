using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос на создание шаблона тела
	/// </summary>
	public class CreateBodyTemplateCommand : IValidatableCommand<BodyTemplate>
	{
		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона тела
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список частей тела
		/// </summary>
		public List<UpdateBodyTemplateRequestItem> BodyTemplateParts { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new RequestFieldNullException<CreateBodyTemplateCommand>(nameof(Name));

			if (BodyTemplateParts is null)
				return;

			var sortedList = BodyTemplateParts.OrderBy(x => x.MinToHit).ToList();

			foreach (var item in BodyTemplateParts.OrderBy(x => x.MinToHit))
			{
				if (string.IsNullOrEmpty(item.Name))
					throw new RequestFieldNullException<CreateBodyTemplateCommand>(nameof(UpdateBodyTemplateRequestItem.Name));

				if (BodyTemplateParts.Count(x => x.Name == item.Name) != 1)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(UpdateBodyTemplateRequestItem.Name),
						$"Значения в поле {nameof(item.Name)} повторяются");

				if (!Enum.IsDefined(item.BodyPartType))
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Неизвестный тип части тела");

				if (item.DamageModifier <= 0)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Модификатор урона должен быть больше нуля.");

				if (item.HitPenalty < 1)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Пенальти на попадание должно быть больше нуля.");

				if (item.MinToHit < 1 || item.MinToHit > 10)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Минимум на попадание должен быть в диапазоне от 1 до 10.");

				if (item.MaxToHit < 1 || item.MaxToHit > 10)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Максимум на попадание должен быть в диапазоне от 1 до 10.");

				if (item.MaxToHit < item.MinToHit)
					throw new RequestFieldIncorrectDataException<CreateBodyTemplateCommand>(nameof(BodyTemplateParts),
						"Максимум на попадание должен быть больше минимума на попадание");
			}

			if (sortedList.First().MinToHit != 1 || sortedList.Last().MaxToHit != 10)
				throw new RequestValidationException($"Значения таблицы попаданий не охватывают необходимый диапазон");

			for (int i = 1; i < sortedList.Count; i++)
				if (sortedList[i].MinToHit != sortedList[i - 1].MaxToHit + 1)
					throw new RequestValidationException($"Значения таблицы попаданий пересекаются");
		}
	}
}
