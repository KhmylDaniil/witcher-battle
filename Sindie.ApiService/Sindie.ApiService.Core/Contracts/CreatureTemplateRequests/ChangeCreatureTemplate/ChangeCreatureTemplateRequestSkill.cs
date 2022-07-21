using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Элемент запроса изменения шаблона существа - навык шаблона существа
	/// </summary>
	public class ChangeCreatureTemplateRequestSkill
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }
		
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
