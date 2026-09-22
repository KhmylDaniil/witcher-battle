using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	/// <summary>Снятие Огня/Падения действием без броска — всегда с себя (см. ConditionRemovalCatalog.IsAutoClearable).</summary>
	public class ClearConditionRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public Condition Condition { get; set; }
	}
}
