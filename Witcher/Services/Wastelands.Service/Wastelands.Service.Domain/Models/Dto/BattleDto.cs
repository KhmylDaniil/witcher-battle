using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Dto
{
	public class BattleDto : BaseDto
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public BattleStatus Status { get; set; }

		public List<BattleCreatureDto> Creatures { get; set; } = [];

		public List<BattleCharacterDto> Characters { get; set; } = [];
	}
}
