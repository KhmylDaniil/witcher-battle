using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemTemplateBase;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class CreateArmorTemplateCommand : CreateOrUpdateItemTemplateCommandBase, IValidatableCommand<Guid>
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor { get; set; }

		/// <summary>
		/// Скованность движений
		/// </summary>
		public int EncumbranceValue { get; set; }

		/// <summary>
		/// Части тела, закрываемые броней
		/// </summary>
		public List<BodyPartType> BodyPartTypes { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public override void Validate()
		{
			base.Validate();

			if (Armor < 1)
				throw new RequestFieldIncorrectDataException<CreateArmorTemplateCommand>(nameof(Armor), "Значение должно быть больше нуля");

			if (EncumbranceValue < 0)
				throw new RequestFieldIncorrectDataException<CreateArmorTemplateCommand>(nameof(EncumbranceValue), "Значение не может быть меньше нуля");

			if (BodyPartTypes is not null && BodyPartTypes.Any())
				foreach (var item in BodyPartTypes)
				{
					if (!Enum.IsDefined(item))
						throw new RequestFieldIncorrectDataException<CreateArmorTemplateCommand>(nameof(BodyPartTypes), "Неизвестный тип части тела");
				}
			else
				throw new RequestFieldNullException<CreateArmorTemplateCommand>(nameof(BodyPartTypes));
		}
	}
}
