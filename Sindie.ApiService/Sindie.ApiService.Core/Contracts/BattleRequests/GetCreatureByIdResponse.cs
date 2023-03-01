using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Ответ на запрос существа по айди
	/// </summary>
	public class GetCreatureByIdResponse
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Название шаблона существа
		/// </summary>
		public string CreatureTemplateName { get; set; }
		
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string BodyTemplateName { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public CreatureType CreatureType { get; set; }

		/// <summary>
		/// Хиты
		/// </summary>
		public (int current, int max) HP { get; set; }

		/// <summary>
		/// Стамина
		/// </summary>
		public (int current, int max) Sta { get; set; }

		/// <summary>
		/// Интеллект
		/// </summary>
		public (int current, int max) Int { get; set; }

		/// <summary>
		/// Рефлексы
		/// </summary>
		public (int current, int max) Ref { get; set; }

		/// <summary>
		/// Ловкость
		/// </summary>
		public (int current, int max) Dex { get; set; }

		/// <summary>
		/// Телосложение
		/// </summary>
		public (int current, int max) Body { get; set; }

		/// <summary>
		/// Эмпатия
		/// </summary>
		public (int current, int max) Emp { get; set; }

		/// <summary>
		/// Крафт
		/// </summary>
		public (int current, int max) Cra { get; set; }

		/// <summary>
		/// Воля
		/// </summary>
		public (int current, int max) Will { get; set; }

		/// <summary>
		/// Удача
		/// </summary>
		public (int current, int max) Luck { get; set; }

		/// <summary>
		/// Скорость
		/// </summary>
		public (int current, int max) Speed { get; set; }

		/// <summary>
		/// Список частей существа
		/// </summary>
		public List<GetCreatureByIdResponsePart> CreatureParts { get; set; }

		/// <summary>
		/// Список навыков существа
		/// </summary>
		public List<GetCreatureByIdResponseSkill> CreatureSkills { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<GetCreatureByIdResponseAbility> Abilities { get; set; }

		/// <summary>
		/// Модификаторы урона
		/// </summary>
		public List<(DamageType DamageType, DamageTypeModifier Modifier)> DamageTypeModifiers { get; set; }
	}
}