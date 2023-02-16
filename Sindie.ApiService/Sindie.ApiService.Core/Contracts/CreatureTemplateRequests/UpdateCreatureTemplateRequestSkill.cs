using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Элемент изменения или создания шаблона существа - навык шаблона существа
	/// </summary>
	public sealed class UpdateCreatureTemplateRequestSkill
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
