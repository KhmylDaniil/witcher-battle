using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleCreatureDto : BaseDto
	{
		public long CreatureTemplateId { get; set; }

		public string Name { get; set; }

		public CreatureType CreatureType { get; set; }

		public int MaxHP { get; set; }

		public int CurrentHP { get; set; }

		public int MaxSta { get; set; }

		public int CurrentSta { get; set; }

		public int Recovery { get; set; }

		public int Stun { get; set; }

		public int? Initiative { get; set; }

		/// <summary>Позиция на подключённой к бою карте; null — не выставлен.</summary>
		public int? MapColumn { get; set; }

		public int? MapRow { get; set; }

		public int MaxMovement { get; set; }

		public int CurrentMovement { get; set; }

		public List<Condition> AppliedConditions { get; set; } = [];

		/// <summary>Износ брони по частям тела в этом бою — для расчёта текущей (эффективной) брони на фронте.</summary>
		public Dictionary<long, int> ArmorReductionByPartId { get; set; } = [];
	}
}
