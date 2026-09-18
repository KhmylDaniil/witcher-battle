using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models
{
	/// <summary>
	/// Всё, что нужно для боевых расчётов про одного участника боя: его способности и функция
	/// "характеристика+навык" (см. SkillHelpers — у Character и CreatureTemplate разные имена
	/// статов при одинаковой группировке навыков). Template заполнен только для существ — нужен
	/// для частей тела/брони/модификаторов урона, которых у персонажей нет.
	/// </summary>
	public class ParticipantCombatContext
	{
		public required List<Ability> Abilities { get; init; }

		public required Func<Skill, int> GetSkillValue { get; init; }

		public CreatureTemplate? Template { get; init; }

		/// <summary>Заполнено только для существ — нужна для чтения/износа брони конкретного экземпляра в этом бою.</summary>
		public Creature? Creature { get; init; }
	}
}
