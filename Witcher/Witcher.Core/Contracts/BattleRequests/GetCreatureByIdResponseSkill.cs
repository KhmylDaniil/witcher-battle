using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Элемент ответа на запрос существа по айди - навык шаблона существа
	/// </summary>
	public sealed class GetCreatureByIdResponseSkill
	{
		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Значение навыка
		/// </summary>
		public (int current, int max) SkillValue { get; set; }
	}
}