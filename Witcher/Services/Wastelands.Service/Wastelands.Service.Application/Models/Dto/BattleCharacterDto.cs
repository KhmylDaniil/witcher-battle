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

		public List<Condition> AppliedConditions { get; set; } = [];
	}
}
