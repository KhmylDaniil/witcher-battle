using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateCommand : IValidatableCommand<CreatureTemplate>
	{
		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public CreatureType CreatureType { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Хиты
		/// </summary>
		public int HP { get; set; } = 10;

		/// <summary>
		/// Стамина
		/// </summary>
		public int Sta { get; set; } = 1;

		/// <summary>
		/// Интеллект
		/// </summary>
		public int Int { get; set; } = 1;

		/// <summary>
		/// Рефлексы
		/// </summary>
		public int Ref { get; set; } = 1;

		/// <summary>
		/// Ловкость
		/// </summary>
		public int Dex { get; set; } = 1;

		/// <summary>
		/// Телосложение
		/// </summary>
		public int Body { get; set; } = 1;

		/// <summary>
		/// Эмпатия
		/// </summary>
		public int Emp { get; set; } = 1;

		/// <summary>
		/// Крафт
		/// </summary>
		public int Cra { get; set; } = 1;

		/// <summary>
		/// Воля
		/// </summary>
		public int Will { get; set; } = 1;

		/// <summary>
		/// Сккорость
		/// </summary>
		public int Speed { get; set; }

		/// <summary>
		/// Удача
		/// </summary>
		public int Luck { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public List<UpdateCreatureTemplateRequestArmorList> ArmorList { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<Guid> Abilities { get; set; }

		/// <summary>
		/// Навыки шаблона существа
		/// </summary>
		public List<UpdateCreatureTemplateRequestSkill> CreatureTemplateSkills { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (!Enum.IsDefined(CreatureType))
				throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(CreatureType));

			if (string.IsNullOrWhiteSpace(Name))
				throw new RequestFieldNullException<CreateCreatureTemplateCommand>(nameof(Name));

			if (HP < 1 || Sta < 1 || Int < 1 || Ref < 1 || Dex < 1 || Body < 1 || Emp < 1 || Cra < 1 || Will < 1)
				throw new RequestValidationException("Значение характеристики должно быть больше нуля");

			if (Speed < 0)
				throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(Speed),
					"Значение характеристики не может быть отрицательным");

			if (Luck < 0)
				throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(Luck),
					"Значение характеристики не может быть отрицательным");

			if (ArmorList is not null)
				foreach (var item in ArmorList)
					if (item.Armor < 1)
						throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(ArmorList),
							"Значение брони должно быть больше нуля");

			if (CreatureTemplateSkills is not null)
				foreach (var item in CreatureTemplateSkills)
				{
					if (item.Value < 1)
						throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(CreatureTemplateSkills),
							"Значение навыка должно быть больше нуля");

					if (!Enum.IsDefined(item.Skill))
						throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(CreatureType));

					if (CreatureTemplateSkills.Count(x => x.Skill == item.Skill) != 1)
						throw new RequestNotUniqException<CreateCreatureTemplateCommand>(nameof(CreatureTemplateSkills));
				}
		}
	}
}
