using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class SetDefenderChoiceRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public Skill DefensiveSkill { get; set; }

		public int? DefenseRoll { get; set; }

		/// <summary>
		/// Парирование вместо обычного защитного навыка — доступно только при экипированном оружии
		/// ближнего боя (см. BattleParticipants.GetEquippedMeleeWeaponSkill). Когда true, DefensiveSkill
		/// игнорируется — фактический навык сервер определяет сам по экипированному оружию.
		/// </summary>
		public bool IsParry { get; set; }
	}
}
