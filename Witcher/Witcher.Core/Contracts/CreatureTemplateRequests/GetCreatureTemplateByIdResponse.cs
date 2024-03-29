﻿using System;
using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Ответ на запрос шаблона существа по айди
	/// </summary>
	public sealed class GetCreatureTemplateByIdResponse
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
		/// Название шаблона
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public CreatureType CreatureType { get; set; }

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
		/// Удача
		/// </summary>
		public int Luck { get; set; }

		/// <summary>
		/// Скорость
		/// </summary>
		public int Speed { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Дата изменения
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Айди создавшего пользователя
		/// </summary>
		public Guid CreatedByUserId { get; set; }

		/// <summary>
		/// Айди изменившего пользователя
		/// </summary>
		public Guid ModifiedByUserId { get; set; }

		/// <summary>
		/// Список частей шаблона существа
		/// </summary>
		public List<GetCreatureTemplateByIdResponseBodyPart> CreatureTemplateParts { get; set; }

		/// <summary>
		/// Список навыков шаблона существа
		/// </summary>
		public List<GetCreatureTemplateByIdResponseSkill> CreatureTemplateSkills { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<GetCreatureTemplateByIdResponseAbility> Abilities { get; set; }

		/// <summary>
		/// Модификаторы урона
		/// </summary>
		public List<GetCreatureTemplateByIdResponseDamageTypeModifier> DamageTypeModifiers { get; set; }
	}
}
