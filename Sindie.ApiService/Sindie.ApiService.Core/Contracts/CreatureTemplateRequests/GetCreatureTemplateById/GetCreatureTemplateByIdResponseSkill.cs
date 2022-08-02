using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
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
		/// Айди навыка
		/// </summary>
		public Guid SkillId { get; set; }

		/// <summary>
		/// Название навыка
		/// </summary>
		public string SkillName { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public int SkillValue { get; set; }
	}
}
