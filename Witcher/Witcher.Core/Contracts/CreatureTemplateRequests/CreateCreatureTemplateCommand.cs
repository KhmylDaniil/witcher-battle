using Witcher.Core.Entities;
using System;
using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateCommand : IRequest<CreatureTemplate>
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
		/// Способности
		/// </summary>
		public List<Guid> Abilities { get; set; }
	}
}
