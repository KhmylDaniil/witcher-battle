using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - навык шаблона существа
	/// </summary>
	public sealed class CreateCreatureTemplateRequestSkill
	{
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
