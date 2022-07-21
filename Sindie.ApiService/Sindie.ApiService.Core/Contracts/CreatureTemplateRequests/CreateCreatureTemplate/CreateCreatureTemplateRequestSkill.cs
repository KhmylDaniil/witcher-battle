using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - навык шаблона существа
	/// </summary>
	public class CreateCreatureTemplateRequestSkill
	{
		/// <summary>
		/// Айди навыка
		/// </summary>
		public Guid SkillId { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public int Value { get; set; } 
	}
}
