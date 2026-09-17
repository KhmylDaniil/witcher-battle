using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleDto : BaseDto
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public BattleStatus Status { get; set; }

		public int CurrentRound { get; set; }

		public int? CurrentInitiative { get; set; }

		public List<BattleCreatureDto> Creatures { get; set; } = [];

		public List<BattleCharacterDto> Characters { get; set; } = [];

		public BattleAttackDto? Attack { get; set; }

		public List<BattleLogEntryDto> LogEntries { get; set; } = [];
	}
}
