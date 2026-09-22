using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class AttemptRemoveConditionRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public Condition Condition { get; set; }

		public Skill Skill { get; set; }

		/// <summary>Цель — кого пытаются вылечить. Для self-only навыков (Endurance) должна совпадать с действующим участником.</summary>
		public ParticipantKind TargetKind { get; set; }

		public long TargetId { get; set; }

		/// <summary>Ручной ввод д10 (бросок может взрываться, поэтому диапазон не ограничен 1-10) — null означает бросок сервером.</summary>
		public int? Roll { get; set; }
	}
}
