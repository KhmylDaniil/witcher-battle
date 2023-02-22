using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Элемент запроса изменения шаблона существа - навык шаблона существа
	/// </summary>
	public sealed class ChangeCreatureTemplateRequestSkill
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }
		
		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public int Value { get; set; }
	}
}
