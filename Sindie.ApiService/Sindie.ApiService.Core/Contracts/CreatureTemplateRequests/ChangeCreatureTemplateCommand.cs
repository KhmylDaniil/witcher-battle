using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;
using System.Linq;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос изменения шаблона существа
	/// </summary>
	public class ChangeCreatureTemplateCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

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
		public int HP { get; set; }

		/// <summary>
		/// Стамина
		/// </summary>
		public int Sta { get; set; }

		/// <summary>
		/// Интеллект
		/// </summary>
		public int Int { get; set; }

		/// <summary>
		/// Рефлексы
		/// </summary>
		public int Ref { get; set; }

		/// <summary>
		/// Ловкость
		/// </summary>
		public int Dex { get; set; }

		/// <summary>
		/// Телосложение
		/// </summary>
		public int Body { get; set; }

		/// <summary>
		/// Эмпатия
		/// </summary>
		public int Emp { get; set; }

		/// <summary>
		/// Крафт
		/// </summary>
		public int Cra { get; set; }

		/// <summary>
		/// Воля
		/// </summary>
		public int Will { get; set; }

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
				throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(CreatureType));

			if (string.IsNullOrWhiteSpace(Name))
				throw new RequestFieldNullException<ChangeCreatureTemplateCommand>(nameof(Name));

			if (HP < 1 || Sta < 1 || Int < 1 || Ref < 1 || Dex < 1 || Body < 1 || Emp < 1 || Cra < 1 || Will < 1)
				throw new RequestValidationException("Значение характеристики должно быть больше нуля");

			if (Speed < 0)
				throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(Speed),
					"Значение характеристики не может быть отрицательным");

			if (Luck < 0)
				throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(Luck),
					"Значение характеристики не может быть отрицательным");

			if (ArmorList is not null)
				foreach (var item in ArmorList)
					if (item.Armor < 1)
						throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(ArmorList),
							"Значение брони должно быть больше нуля");

			if (CreatureTemplateSkills is not null)
				foreach (var item in CreatureTemplateSkills)
				{
					if (item.Value < 1)
						throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(CreatureTemplateSkills),
							"Значение навыка должно быть больше нуля");

					if (!Enum.IsDefined(item.Skill))
						throw new RequestFieldIncorrectDataException<ChangeCreatureTemplateCommand>(nameof(CreatureType));

					if (CreatureTemplateSkills.Count(x => x.Skill == item.Skill) != 1)
						throw new RequestNotUniqException<ChangeCreatureTemplateCommand>(nameof(CreatureTemplateSkills));
				}
		}
	}
}
