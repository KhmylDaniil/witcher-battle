using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleCharacterDto : BaseDto
	{
		public long CharacterId { get; set; }

		public string CharacterName { get; set; }

		/// <summary>Владелец персонажа — нужен фронтенду, чтобы понять, кто контролирует эту сторону боя.</summary>
		public long CharacterUserId { get; set; }

		public int MaxHP { get; set; }

		public int CurrentHP { get; set; }

		public int MaxSta { get; set; }

		public int CurrentSta { get; set; }

		public int? Initiative { get; set; }

		/// <summary>Позиция на подключённой к бою карте; null — не выставлен.</summary>
		public int? MapColumn { get; set; }

		public int? MapRow { get; set; }

		public List<Condition> AppliedConditions { get; set; } = [];

		/// <summary>true — этот персонаж уже потратил в свой текущий ход основное действие и может взять дополнительное (см. BattleAttack.BonusActionStaminaCost) или закончить ход.</summary>
		public bool HasActedThisTurn { get; set; }
	}
}
