using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - навык шаблона существа
	/// </summary>
	public sealed class GetCreatureTemplateByIdResponseSkill
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public int SkillValue { get; set; }
	}
}
