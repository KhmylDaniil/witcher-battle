using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Запрос создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public string Type { get; set; }

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
		public List<CreateCreatureTemplateRequestArmorList> ArmorList { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<CreateCreatureTemplateRequestAbility> Abilities { get; set; }

		/// <summary>
		/// Параметры шаблона существа
		/// </summary>
		public List<CreateCreatureTemplateRequestParameter> CreatureTemplateParameters { get; set; }
	}
}
